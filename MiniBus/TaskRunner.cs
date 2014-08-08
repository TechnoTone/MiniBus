//using System;
//using System.Collections.ObjectModel;
//using System.Threading.Tasks;
//
//namespace MiniBus
//{
//  internal class TaskRunner
//  {
//    private readonly Action<AggregateException> exceptionHandler;
//    private readonly Collection<Task> tasks;
//    //    private Task monitorTask;
//
//    public TaskRunner(Action<AggregateException> exceptionHandler)
//    {
//      this.exceptionHandler = exceptionHandler;
//      tasks = new Collection<Task>();
//    }
//
//    //    private List<Task> completedTasks()
//    //    {
//    //      return tasks.Where(t => t.IsCompleted).ToList();
//    //    }
//
//    private void handleTaskExceptions(Task task)
//    {
//      if (task.Exception != null)
//        exceptionHandler(task.Exception);
//    }
//
//    public void NewTask(Action action)
//    {
//      Task.Factory.StartNew(action)
//        .ContinueWith(checkForException);
//    }
//
//    private void checkForException(Task task)
//    {
//      if (task.IsFaulted)
//        handleTaskExceptions(task);
//    }
//  }
//
//  internal class TaskMonitor
//  {
//    private readonly Action action;
//    private readonly Action<Exception> exceptionListener;
//
//    public TaskMonitor(Action action, Action<Exception> exceptionListener)
//    {
//      this.action = action;
//      this.exceptionListener = exceptionListener;
//      //Task.Factory.StartNew(action).ContinueWith(checkForException);
//
//      runAction();
//    }
//
//    private void runAction()
//    {
//      try
//      {
//        action.Invoke();
//      }
//      catch (Exception ex)
//      {
//        exceptionListener(ex);
//      }
//    }
//
//    //    private void checkForException(Task task)
//    //    {
//    //      if (task.Exception != null)
//    //        exceptionListener(task.Exception);
//    //    }
//  }
//}
