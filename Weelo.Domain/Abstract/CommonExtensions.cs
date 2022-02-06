namespace Weelo.Domain.Abstract;

/// <summary>
/// Common extensions
/// </summary>
public static class CommonExtensions
{
    /// <summary>
    /// verify if a string variable is null or empty
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
    /// <summary>
    /// verify if an object is null
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNull(this object value) => value == null;
    /// <summary>
    /// verify if an object is not null
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNotNull(this object value) => value != null;
    /// <summary>
    /// verify if an enumerable is null or not any
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNullOrNotAny<T>(this IEnumerable<T> value) => value == null || !value.Any();
    /// <summary>
    /// verify if an enumerable is not null and any
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNotNullAndAny<T>(this IEnumerable<T> value) => value != null && value.Any();
    /// <summary>
    /// makes a join of a string with an enumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string Join<T>(this IEnumerable<T> value, string separator) => string.Join(separator, value);
    /// <summary>
    /// convert an object to int
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int AsInt(this object value) => Convert.ToInt32(value);
    /// <summary>
    /// convert to long
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static long AsLong(this object value) => Convert.ToInt64(value);
    /// <summary>
    /// convert to short
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static short AsShort(this object value) => Convert.ToInt16(value);
    /// <summary>
    /// convert to decimal
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static decimal AsDecimal(this object value) => Convert.ToDecimal(value);
    /// <summary>
    /// convert to string
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string AsString(this object value) => Convert.ToString(value)!;
    /// <summary>
    /// convert to byte
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte AsByte(this object value) => Convert.ToByte(value);
}

