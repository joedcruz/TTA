using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TTAServer
{
    public class SettingsController : Controller
    {
        #region Protected Members

        protected TTADbContext ttaDbContext;

        #endregion

        public SettingsController(TTADbContext context)
        {
            ttaDbContext = context;
        }

        [Route("api/addsetting")]
        public async Task<IActionResult> AddSetting([FromBody] SettingsDataModel settingsDataModel)
        {
            var setting = new SettingsDataModel(settingsDataModel.Name, settingsDataModel.Value);
            await ttaDbContext.Settings.AddAsync(setting);
            await ttaDbContext.SaveChangesAsync();
            return Ok();
        }

        [Route("api/getsettings")]
        public async Task<List<SettingsDataModel>> GetSettings()
        {
            List<SettingsDataModel> settings = new List<SettingsDataModel>();
            var result = await ttaDbContext.Settings.ToListAsync();

            foreach (var setting in result)
            {
                settings.Add(new SettingsDataModel(setting.Id, setting.Name, setting.Value));
            }

            return settings;
        }

        [Route("api/updatesetting")]
        public async Task<IActionResult> UpdateSetting([FromBody] SettingsDataModel settingsDataModel)
        {
            var setting = new SettingsDataModel();

            setting = await ttaDbContext.Settings.FindAsync(settingsDataModel.Id);

            if (setting != null)
            {
                setting.Name = settingsDataModel.Name;
                setting.Value = settingsDataModel.Value;
            }

            await ttaDbContext.SaveChangesAsync();
            return Ok();
        }

        [Route("api/deletesetting")]
        public async Task<IActionResult> DeleteSetting([FromBody] int id)
        {
            var setting = await ttaDbContext.Settings.FindAsync(id);
            ttaDbContext.Settings.Remove(setting);
            await ttaDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
