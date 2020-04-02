using System.ComponentModel.DataAnnotations.Schema;

namespace Qsor.Database.Models
{
    [Table("beatmaps")]
    public class BeatmapModel
    {
        public int Id { get; set; }
        public string File { get; set; } // this.osu
        public string Path { get; set; } // /Songs/.../this.osu

        public string Audio { get; set; }
        public string Thumbnail { get; set; }
        
        // TODO: Difficulty and other information pre-cached.
    }
}