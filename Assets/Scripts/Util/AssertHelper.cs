using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Util;

public static class AssertHelper
{
    [Conditional("UNITY_EDITOR")]
    public static void Assert(
        [DoesNotReturnIf(false)] bool condition,
        string message = "",
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (condition)
            return;

        throw new AssertionFailedException(message, memberName, sourceFilePath, sourceLineNumber);
    }
}

public class AssertionFailedException : Exception
{
    public AssertionFailedException(string message, string memberName, string sourceFilePath, int sourceLineNumber) :
        base(BuildComplexMessage(message, memberName, sourceFilePath, sourceLineNumber))
    {
    }

    private static string BuildComplexMessage(string message, string memberName, string sourceFilePath, int sourceLineNumber)
    {
        var sb = new StringBuilder();

        sb.AppendLine(string.IsNullOrEmpty(message)
            ? "Assertion failed."
            : $"Assertion failed: {message}");

        sb.Append($" at {memberName}   {sourceFilePath}:{sourceLineNumber}");

        var finalMessage = sb.ToString();
        return finalMessage;
    }
}