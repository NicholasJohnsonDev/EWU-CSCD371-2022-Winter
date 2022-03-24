// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "INTL0003:Methods PascalCase", Justification = "Underscores allowed in test names.")]
[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Underscores allowed in test names.")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Globalization not required in this assignment.")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "Globalization not required in this assignment.")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Suppressing warning in provided file")]
[assembly: SuppressMessage("Naming", "INTL0001:Fields _PascalCase", Justification = "Suppressing warning in provided file")]
[assembly: SuppressMessage("Performance", "CA1849:Call async methods when in an async method", Justification = "No, I'm trying to throw an exception!")]
[assembly: SuppressMessage("Globalization", "CA1310:Specify StringComparison for correctness", Justification = "Suppressing warning in provided file")]
[assembly: SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Suppressing warning in provided file")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Suppressing warning in provided file")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Need to do it per assignment directions")]
