using System.ComponentModel.DataAnnotations;

/// <summary>
/// Not used in this project. However a table is created in the database during the test run
/// </summary>
namespace TTAServer
{
    /// <summary>
    /// Our settings database table representational model
    /// </summary>
    public class SettingsDataModel
    {
        /// <summary>
        /// The unique Id for this entry
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The settings name
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// The settings value
        /// </summary>
        [Required]
        [MaxLength(2048)]
        public string Value { get; set; }

        public SettingsDataModel()
        {

        }

        public SettingsDataModel(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public SettingsDataModel(int id, string name, string value)
        {
            this.Id = id;
            this.Name = name;
            this.Value = value;
        }
    }
}
