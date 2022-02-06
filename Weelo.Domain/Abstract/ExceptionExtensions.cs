using System.Text;

namespace Weelo.Domain.Abstract
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Convert an exception to detailed string
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string ToDetailedString(this Exception exception)
        {
            if (exception.GetType().Name.Equals("DbEntityValidationException"))
            {
                StringBuilder errors = new();
                foreach (var eve in ((dynamic)exception).EntityValidationErrors)
                {
                    errors.AppendLine(string.Format("Entity of type {0} in state {1} has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));

                    foreach (var ve in eve.ValidationErrors)
                    {
                        errors.AppendLine(string.Format("- Property: {0}, Error: {1}", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                return errors.ToString();
            }
            else
            {
                var properties = exception.GetType()
                    .GetProperties();
                var fields = properties.Select(property => new
                {
                    property.Name,
                    Value = property.GetValue(exception, null)
                })
                    .Select(x => string.Format(
                        "{0} : {1}", x.Name, x.Value != null ? x.Value.ToString() : string.Empty
                     ));

                return fields.Join("\n");
            }
        }
    }
}
