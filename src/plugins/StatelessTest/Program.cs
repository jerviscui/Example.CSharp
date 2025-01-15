using Stateless;

namespace StatelessTest
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            var machine = new StateMachine<ChannelState, Trigger>(ChannelState.WaitState);

            machine.Configure(ChannelState.WaitState)
                .Permit(Trigger.ReceiveNumber, ChannelState.RecognizingState);

            //machine.Configure(ChannelState.RecognizingState)
            //    .Permit(Trigger.IsEnter, ChannelState.ReadyEnterState)
            //    .Permit(Trigger.IsLeave, ChannelState.ReadyLeavingState)
            //    .Permit(Trigger.RecognizeFailed, ChannelState.WaitState);

            machine.Configure(ChannelState.ReadyEnterState)
                .Permit(Trigger.OpenDoor, ChannelState.EnteringState);

            machine.Configure(ChannelState.ReadyLeavingState)
                .Permit(Trigger.OpenDoor, ChannelState.LeavingState);

            machine.Configure(ChannelState.EnteringState)
                .Permit(Trigger.Passed, ChannelState.WaitState);

            machine.Configure(ChannelState.LeavingState)
                .Permit(Trigger.Passed, ChannelState.WaitState);

            //System.InvalidOperationException
            //Message = No valid leaving transitions are permitted from state 'WaitState' for trigger 'IsLeave'.Consider ignoring the trigger.
            machine.Configure(ChannelState.WaitState)
                .Ignore(Trigger.IsLeave);
            machine.Fire(Trigger.IsLeave);

            //System.InvalidOperationException
            //Message = Parameters for the trigger 'Stateless.StateMachine`2+TriggerWithParameters`1[StatelessTest.Program+ChannelState,StatelessTest.Program+Trigger,StatelessTest.Program+RecognizeEventData]' have already been configured.
            machine.Configure(ChannelState.RecognizingState)
                .PermitIf(machine.SetTriggerParameters<RecognizeEventData>(Trigger.ReceiveNumber),
                    ChannelState.ReadyEnterState, data => data.IsEnter && data.Direction != Direction.OnlyOut)
                //.PermitIf(machine.SetTriggerParameters<RecognizeEventData>(Trigger.ReceiveNumber),
                //    ChannelState.ReadyLeavingState, data => !data.IsEnter && data.Direction != Direction.OnlyIn)
                .Permit(Trigger.RecognizeFailed, ChannelState.WaitState);

            var eventData = new RecognizeEventData { IsEnter = true, Direction = Direction.OnlyOut };
            machine.Fire(
                new StateMachine<ChannelState, Trigger>.TriggerWithParameters<RecognizeEventData>(
                    Trigger.ReceiveNumber), eventData);
        }

        public class RecognizeEventData
        {
            public bool IsEnter { get; set; }

            public Direction Direction { get; set; }
        }

        public enum Direction
        {
            OnlyIn,

            OnlyOut,

            Bothway
        }

        private enum ChannelState
        {
            WaitState,

            RecognizingState,

            ReadyEnterState,

            EnteringState,

            ReadyLeavingState,

            LeavingState
        }

        private enum Trigger
        {
            ReceiveNumber,

            RecognizeFailed,

            IsEnter,

            IsLeave,

            OpenDoor,

            Passed
        }
    }
}
