using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Services.Controllers.API.Configuration;

public class DiagnosticObserver : IObserver<DiagnosticListener>
{
  public void OnCompleted()
      => throw new NotImplementedException();

  public void OnError(Exception error)
      => throw new NotImplementedException();

  public void OnNext(DiagnosticListener value)
  {
    if (value.Name == DbLoggerCategory.Name) // "Microsoft.EntityFrameworkCore"
    {
#pragma warning disable CS8620
      value.Subscribe(new KeyValueObserver());
#pragma warning restore CS8620
    }
  }
}

public class KeyValueObserver : IObserver<KeyValuePair<string, object>>
{
  public void OnCompleted()
      => throw new NotImplementedException();

  public void OnError(Exception error)
      => throw new NotImplementedException();

  public void OnNext(KeyValuePair<string, object> value)
  {
    if (value.Key == CoreEventId.ContextInitialized.Name)
    {
      var payload = (ContextInitializedEventData)value.Value;
      Console.WriteLine($"===> 💻 EF is initializing {payload.Context.GetType().Name} ");
    }

    if (value.Key == CoreEventId.ContextDisposed.Name)
    {
      var payload = (DbContextEventData)value.Value;
      Console.WriteLine($"===> 💻 EF is disposing {payload.Context?.GetType().Name} ");
    }

    if (value.Key == RelationalEventId.ConnectionOpening.Name)
    {
      var payload = (ConnectionEventData)value.Value;
      Console.WriteLine($"===> 💻 EF is opening a connection to {payload.Connection.ConnectionString} ");
    }

    if (value.Key == RelationalEventId.ConnectionClosing.Name)
    {
      var payload = (ConnectionEventData)value.Value;
      Console.WriteLine($"===> 💻 EF is closing the connection to {payload.Connection.ConnectionString} ");
    }
  }
}
