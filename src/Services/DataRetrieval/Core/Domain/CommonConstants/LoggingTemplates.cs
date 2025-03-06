using System.Text;

namespace Domain.CommonConstants;
public static class LoggingTemplates
{
    public static readonly CompositeFormat ErrorTemplate =
        CompositeFormat.Parse("An exception occured. Message:{0}. StackTrace:{1}.");
}
