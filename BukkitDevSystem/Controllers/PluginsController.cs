using BukkitDevSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BukkitDevSystem.Controllers
{
    public class PluginsController : Controller
    {
        private readonly BukkitDevSystemContext _context;

        public PluginsController(BukkitDevSystemContext context)
        {
            _context = context;
        }

        // GET: Plugins
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pluginlist.ToListAsync());
        }

        // GET: Plugins/Gratuitos
        public async Task<IActionResult> Gratuitos()
        {
            var plugins = await _context.Pluginlist.ToListAsync();
            var gratuitos = plugins.Where(plugin => plugin.TipoPlugin == "Gratuito");
            return View("Index", gratuitos);
        }

        // GET: Plugins/Pagos
        public async Task<IActionResult> Pagos()
        {
            var plugins = await _context.Pluginlist.ToListAsync();
            var gratuitos = plugins.Where(plugin => plugin.TipoPlugin == "Pago");
            return View("Index", gratuitos);
        }


        // GET: Plugins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pluginModel = await _context.Pluginlist
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pluginModel == null)
            {
                return NotFound();
            }

            return View(pluginModel);
        }

        // GET: Plugins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Plugins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomePlugin,AutorPlugin,VersaoPlugin,TipoPlugin,PrecoPlugin,DescricaoPlugin,ImagemPadraoPersonalizada,Id")] PluginModel pluginModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pluginModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pluginModel);
        }

        // GET: Plugins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pluginModel = await _context.Pluginlist.FindAsync(id);
            if (pluginModel == null)
            {
                return NotFound();
            }
            return View(pluginModel);
        }

        // POST: Plugins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NomePlugin,AutorPlugin,VersaoPlugin,TipoPlugin,PrecoPlugin,DescricaoPlugin,ImagemPadraoPersonalizada,Id")] PluginModel pluginModel)
        {
            if (id != pluginModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pluginModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PluginModelExists(pluginModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pluginModel);
        }

        // GET: Plugins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pluginModel = await _context.Pluginlist
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pluginModel == null)
            {
                return NotFound();
            }

            return View(pluginModel);
        }

        // POST: Plugins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pluginModel = await _context.Pluginlist.FindAsync(id);
            _context.Pluginlist.Remove(pluginModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PluginModelExists(int id)
        {
            return _context.Pluginlist.Any(e => e.Id == id);
        }
    }
}
