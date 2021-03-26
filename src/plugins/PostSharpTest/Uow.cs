using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;

namespace PostSharpTest
{
    public interface IUow
    {
        public void Create()
        {
            Console.WriteLine("Create");
        }

        public void Commit()
        {
            Console.WriteLine("Commit");
        }

        public void Rollback()
        {
            Console.WriteLine("Rollback");
        }
    }

    public class Uow : IUow
    {
    }

    [Serializable]
    public class UowMethodAspect : MethodInterceptionAspect, IUow
    {
        /// <summary>
        ///   Method invoked <i>instead</i> of the method to which the aspect has been applied.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            ((IUow)this).Create();

            try
            {
                args.Invoke(args.Arguments);

                ((IUow)this).Commit();
            }
            catch (Exception e)
            {
                ((IUow)this).Rollback();

                throw;
            }
        }

        /// <summary>
        ///   Method invoked <i>instead</i> of the method to which the aspect has been applied.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public override Task OnInvokeAsync(MethodInterceptionArgs args)
        {
            return Task.Run(() => OnInvoke(args));
        }
    }

    [Serializable]
    public class UowBoundaryAspect : OnMethodBoundaryAspect, IUow
    {
        /// <summary>
        ///   Method executed <b>before</b> the body of methods to which this aspect is applied.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed, which are its arguments, and how should the execution continue
        /// after the execution of <see cref="M:PostSharp.Aspects.IOnMethodBoundaryAspect.OnEntry(PostSharp.Aspects.MethodExecutionArgs)" />.</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            ((IUow)this).Create();
        }

        /// <summary>
        ///   Method executed <b>after</b> the body of methods to which this aspect is applied,
        ///   but only when the method successfully returns (i.e. when no exception flies out
        ///   the method.).
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        public override void OnSuccess(MethodExecutionArgs args)
        {
            ((IUow)this).Commit();
        }

        /// <summary>
        ///   Method executed <b>after</b> the body of methods to which this aspect is applied,
        ///   in case that the method resulted with an exception.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        public override void OnException(MethodExecutionArgs args)
        {
            ((IUow)this).Rollback();
        }

        /// <summary>
        ///   Method executed <b>after</b> the body of methods to which this aspect is applied,
        ///   even when the method exists with an exception (this method is invoked from
        ///   the <c>finally</c> block).
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine("OnExit");
        }
    }

    
    [Serializable]
    public class UowBoundaryAspect2 : OnMethodBoundaryAspect, IUow
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            ((IUow)this).Create();
        }
        
        public override void OnSuccess(MethodExecutionArgs args)
        {
            ((IUow)this).Commit();
        }
        
        public override void OnException(MethodExecutionArgs args)
        {
            ((IUow)this).Rollback();
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine("UowBoundaryAspect2 OnExit");
        }

        public void Create()
        {
            Console.WriteLine("UowBoundaryAspect2 Create");
        }

        public void Commit()
        {
            Console.WriteLine("UowBoundaryAspect2 Commit");
        }

        public void Rollback()
        {
            Console.WriteLine("UowBoundaryAspect2 Rollback");
        }
    }

    [Serializable]
    public class UowMethodAspect2 : MethodInterceptionAspect, IUow
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            ((IUow)this).Create();

            try
            {
                args.Invoke(args.Arguments);

                ((IUow)this).Commit();
            }
            catch (Exception e)
            {
                ((IUow)this).Rollback();

                throw;
            }
        }

        public void Create()
        {
            Console.WriteLine("UowMethodAspect2 Create");
        }

        public void Commit()
        {
            Console.WriteLine("UowMethodAspect2 Commit");
        }

        public void Rollback()
        {
            Console.WriteLine("UowMethodAspect2 Rollback");
        }
    }
}
