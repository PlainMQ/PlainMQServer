using PlainMQServer.Models;
using PlainMQServer.Services;
using PlainMQServer.ThreadManagement;
using System.Text;

string[] cmdArgs = Environment.GetCommandLineArgs();

MainService Main = new(cmdArgs);

ManagedThreadPool.Init();

ManagedThreadBase MainThread = new()
{
    Action = (object? o) => Main?.MainAction?.Invoke(o),

    ID = 0,

    InvokeClass = PlainMQServer.Models.Enums.ThreadClass.MAIN,
    Name = "MainThread",
};

ManagedThreadPool.AddToPool(MainThread);

ManagedThreadPool.Broadcast(new ThreadEvent
{
    Class = PlainMQServer.Models.Enums.ThreadClass.MAIN,
    EventPayload = new PlainMessage(Encoding.UTF8.GetBytes("INITED"))
});