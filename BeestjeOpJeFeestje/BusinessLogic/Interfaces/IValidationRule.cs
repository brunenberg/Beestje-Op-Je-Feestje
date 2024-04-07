using BusinessLogic;

public interface IValidationRule {
    (bool isValid, string errorMessage) Validate(ValidationContext context);
}