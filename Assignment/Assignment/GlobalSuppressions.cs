// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Globalization not required in this assignment.")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Globalization not required in this assignment.")]
[assembly: SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "Naming convention not required here")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposal of resources is out of scope of assignment")]
[assembly: SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Leaving provided code alone")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "I don't know what this is :)")]
[assembly: SuppressMessage("Performance", "CA1849:Call async methods when in an async method", Justification = "I need to do this per the directions")]
