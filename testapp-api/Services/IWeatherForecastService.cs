namespace Api.Services;

public interface IWeatherForecastService<T>
{
    public IEnumerable<T> execute();
}