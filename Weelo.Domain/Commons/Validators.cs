using System.Text.RegularExpressions;

namespace Weelo.Domain.Commons
{
    public class Validators
    {
        private static readonly Regex DataUriPattern = new Regex(@"^data\:(?<type>image\/(png|tiff|jpg|jpeg|gif));base64,(?<data>[A-Z0-9\+\/\=]+)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        /// <summary>
        /// Validate image from base64
        /// </summary>
        /// <param name="base64image"></param>
        /// <returns></returns>
        public static bool IsImageFormat(string base64image)
        {
            if (string.IsNullOrWhiteSpace(base64image)) return false;

            Match match = DataUriPattern.Match(base64image);

            if (!match.Success) return false;

            return true;
        }
    }
}
