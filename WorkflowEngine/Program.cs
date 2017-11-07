using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace WorkflowEngine
{
    public class UploadVideo : IActivity
    {
        public void Execute()
        {
            Console.WriteLine("Uploading video");
        }
    }

    public class VideoReadyForEncoding : IActivity
    {
        public void Execute()
        {
            Console.WriteLine("Video ready for encoding ");
        }
    }

    public class NotifyOwner : IActivity
    {
        public void Execute()
        {
            Console.WriteLine("Notifying owner start of processing");
        }
    }

    public class DbUpdateStatus : IActivity
    {
        public void Execute()
        {
            Console.WriteLine("Updating database with video status");
        }
    }


    public interface IActivity
    {
        void Execute();
    }

    public interface IWorkFlow
    {
        void Add(IActivity activity);
        void Remove(IActivity activity);
        IEnumerable<IActivity> GetTasks();
    }

    public class Workflow : IWorkFlow
    {
        private readonly List<IActivity> _activities;

        public Workflow()
        {
            _activities = new List<IActivity>();
        }

        public void Add(IActivity activity)
        {
            _activities.Add(activity);
        }

        public void Remove(IActivity activity)
        {
            _activities.Remove(activity);
        }

        public IEnumerable<IActivity> GetTasks()
        {
            return _activities;
        }
    }

    public class WorkFlowEngine
    {
        public void Run(IWorkFlow workFlow)
        {
            foreach (var work in workFlow.GetTasks())
            {
                try
                {
                    work.Execute();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var videoUploadWorkflow = new Workflow();
            videoUploadWorkflow.Add(new UploadVideo());
            videoUploadWorkflow.Add(new VideoReadyForEncoding());
            videoUploadWorkflow.Add(new NotifyOwner());
            videoUploadWorkflow.Add(new DbUpdateStatus());

            var engine = new WorkFlowEngine();
            engine.Run(videoUploadWorkflow);
        }
    }
}
