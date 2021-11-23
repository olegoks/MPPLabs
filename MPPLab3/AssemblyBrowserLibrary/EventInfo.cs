using System.Reflection;

namespace AssemblyBrowserLibrary.Extensions
{
    public static class EventInfoExtensions
    {
        public static Node GetNode(this EventInfo eventInfo)
        {
            var fieldType = eventInfo.EventHandlerType.ToGenericTypeString();
            var fullType = eventInfo.EventHandlerType.FullName;
            var name = eventInfo.Name;
            return new Node("[event]", type: fieldType, fullType: fullType, name: name);
        }
    }
}