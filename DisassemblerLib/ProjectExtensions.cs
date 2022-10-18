using System.Linq;

namespace Lag.DisassemblerLib
{
    public static class ProjectExtensions
    {
        public static Word FromAbsolute(this IProject project, uint location) => new Word(project.GetSegment(location), location);

        public static Segment GetSegment(this IProject project, uint location) => project.Segments.First(seg => seg.Contains(location));
    }
}
