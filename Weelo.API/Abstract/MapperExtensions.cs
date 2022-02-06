using AutoMapper;
using Weelo.Domain.Abstract;

namespace Weelo.API.Abstract;

/// <summary>
/// Automapper extensions
/// </summary>
public static class MapperExtensions
{
    #region AutoMapper
    /// <summary>
    /// Resuelve el mapeo de dos entidades
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static TDestination? To<TSource, TDestination>(
        this IMapper mapper, TSource source
    )
        where TSource : class
        where TDestination : class
    {
        if (source.IsNull()) return null;

        var map = mapper.ConfigurationProvider.FindTypeMapFor(typeof(TSource), typeof(TDestination));

        if (source.GetType().Name.Equals(typeof(System.Collections.Generic.List<>).Name))
        {
            map = mapper.ConfigurationProvider.FindTypeMapFor(source.GetType().GenericTypeArguments[0], typeof(TDestination).GenericTypeArguments[0]);
        }

        if (map.IsNull())
        {
            MapperConfiguration cnf = new(cfg =>
            {
                if (source.GetType().IsGenericType)
                {
                    cfg.CreateMap(source.GetType().GenericTypeArguments[0], typeof(TDestination).GenericTypeArguments[0]);
                }

                if (!source.GetType().Name.Equals(typeof(System.Collections.Generic.List<>).Name))
                {
                    cfg.CreateMap<TSource, TDestination>();
                }

                cfg.AllowNullDestinationValues = true;
            });

            return cnf.CreateMapper().Map<TSource, TDestination>(source);
        }

        return mapper.Map<TDestination>(source);
    }

    /// <summary>
    /// Resuelve el mapeo de dos entidades
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static TDestination To<TDestination>(
        this IMapper mapper, object source
    )
        where TDestination : class
    {
        if (source == null) return null;

        var map = mapper.ConfigurationProvider.FindTypeMapFor(source.GetType(), typeof(TDestination));

        if (source.GetType().Name.Equals(typeof(System.Collections.Generic.List<>).Name))
        {
            map = mapper.ConfigurationProvider.FindTypeMapFor(source.GetType().GenericTypeArguments[0], typeof(TDestination).GenericTypeArguments[0]);
        }

        if (map == null)
        {
            MapperConfiguration cnf = new(cfg =>
            {
                if (source.GetType().IsGenericType)
                {
                    cfg.CreateMap(source.GetType().GenericTypeArguments[0], typeof(TDestination).GenericTypeArguments[0]);
                }

                if (!source.GetType().Name.Equals(typeof(System.Collections.Generic.List<>).Name))
                {
                    cfg.CreateMap(source.GetType(), typeof(TDestination));
                }

                cfg.AllowNullDestinationValues = true;
            });

            return cnf.CreateMapper().Map<TDestination>(source);
        }

        return mapper.Map<TDestination>(source);
    }

    /// <summary>
    /// Resuel ve un mapeo
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static TDestination? To<TDestination>(this IMapper mapper, object source, Type sourceType, Type destinityType)
    {
        var map = mapper.ConfigurationProvider.FindTypeMapFor(sourceType, destinityType);

        if (source.GetType().Name.Equals(typeof(System.Collections.Generic.List<>).Name))
        {
            map = mapper.ConfigurationProvider.FindTypeMapFor(source.GetType().GenericTypeArguments[0], typeof(TDestination).GenericTypeArguments[0]);

        }
        if (map == null)
        {
            MapperConfiguration cnf = new(cfg =>
            {
                if (source.GetType().IsGenericType)
                {
                    cfg.CreateMap(sourceType.GenericTypeArguments[0], destinityType.GenericTypeArguments[0]);
                }

                if (!source.GetType().Name.Equals(typeof(System.Collections.Generic.List<>).Name))
                {
                    cfg.CreateMap(source.GetType(), typeof(TDestination));
                }

                cfg.AllowNullDestinationValues = true;
            });

            return cnf.CreateMapper().Map<TDestination>(source);
        }

        return mapper.Map<TDestination>(source);
    }
    #endregion
}

