namespace App.Services;

public interface ISampleProducer<T, R>
{
    public R send(T message);
}