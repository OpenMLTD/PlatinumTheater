using JetBrains.Annotations;

namespace PlatiumTheater.Editor.Internal.Vmd {
    public sealed class IKControl {

        internal IKControl() {
        }

        [NotNull]
        public string Name { get; internal set; }

        public bool Enabled { get; internal set; }

    }
}
