using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.API.Models.Shared.Payments;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Checkout.PaymentGateway.API.Features.Payments.Validation
{
    public class ProcessPaymentRequestValidator : Validator<ProcessPaymentRequest>
    {
        private const int CVVCeiling = 10000;
        private const int CVVFloor = 99;
        private const decimal MinPaymentAmount = 0.1m;
        private const decimal MaxPaymentAmount = 10000.0m;
        private const int MaxMonth = 12;

        public ProcessPaymentRequestValidator()
        {
            RuleFor(x => x.CardDetails).NotNull().SetValidator(new CardDtoValidator());
            RuleFor(x => x.Value).NotNull().SetValidator(new PaymentDtoValidator());
            RuleFor(x => x.TransactionTimeStamp).NotNull().SetValidator(new TransactionTimeStampDtoValidator());
        }

        private class TransactionTimeStampDtoValidator : Validator<TransactionTimeStampDto>
        {
            public TransactionTimeStampDtoValidator()
            {
                RuleFor(x => x.TimeStamp).Must(BeValidDate).WithMessage("{PropertyName} Must be a valid date time")
                    .WithName(nameof(TransactionTimeStampDto.TimeStamp));
                RuleFor(x => x.TimeStamp).Must(BeToday).WithMessage("{PropertyName} Must be todays date time")
                    .WithName(nameof(TransactionTimeStampDto.TimeStamp));
                RuleFor(x => x.TimeStamp).Must(BePrecise).WithMessage("{PropertyName} Must be a precise")
                    .WithName(nameof(TransactionTimeStampDto.TimeStamp));
            }

            private bool BeValidDate(DateTime dateTime)
            {
                return !dateTime.Equals(default);
            }

            private bool BePrecise(DateTime dateTime)
            {
                return !dateTime.TimeOfDay.Equals(default);
            }

            private bool BeToday(DateTime dateTime)
            {
                return dateTime.Date.Equals(DateTime.Today);
            }
        }

        private class CardDtoValidator : Validator<CardDto>
        {
            public CardDtoValidator()
            {
                RuleFor(x => x.CVV).GreaterThan(CVVFloor).LessThan(CVVCeiling);
                RuleFor(x => x.Number).NotEmpty().Must(new CreditCardAttribute().IsValid)
                    .WithMessage("Card number is not valid");
                RuleFor(x => x.Expiration).NotNull().SetValidator(new CardExpirationDateDtoValidator());
            }
        }

        private class PaymentDtoValidator : Validator<PaymentDto>
        {
            public PaymentDtoValidator()
            {
                RuleFor(x => x.Amount).GreaterThanOrEqualTo(MinPaymentAmount);
                RuleFor(x => x.Amount).LessThanOrEqualTo(MaxPaymentAmount);
                RuleFor(x => x.ISOCurrencyCode)
                    .Must(BeValidISOCurrencyCode)
                    .WithMessage("{PropertyName} Must be valid ISO currency code")
                    .WithName(nameof(PaymentDto.ISOCurrencyCode));
            }

            private bool BeValidISOCurrencyCode(string isoCode)
            {
                CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

                foreach (CultureInfo ci in cultures)
                {
                    RegionInfo ri = new RegionInfo(ci.LCID);
                    if (ri.ISOCurrencySymbol == isoCode)
                    {
                        return true;
                    }
                }
                return false;
            }

        }

        private class CardExpirationDateDtoValidator : Validator<CardExpirationDateDto>
        {
            public CardExpirationDateDtoValidator()
            {
                RuleFor(x => x.Year).GreaterThanOrEqualTo(DateTime.UtcNow.Year).DependentRules(() =>
                {
                    RuleFor(x => x.Month).GreaterThanOrEqualTo(DateTime.UtcNow.Month).LessThanOrEqualTo(MaxMonth)
                    .DependentRules(() =>
                    {
                        RuleFor(x => new DateTime(x.Year, x.Month, 1)).Must(BeInTheFuture).WithMessage("Card expired");
                    });
                });
            }

            private bool BeInTheFuture(DateTime dateTime)
            {
                return dateTime > DateTime.UtcNow.Date;
            }
        }
    }
}
