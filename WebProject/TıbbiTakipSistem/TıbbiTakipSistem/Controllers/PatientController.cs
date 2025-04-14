using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TıbbiTakipSistem.Data;
using TıbbiTakipSistem.Models;
using System.Security.Claims;

namespace TıbbiTakipSistem.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        }

        // Hastalıklar
        public async Task<IActionResult> MyDiseases()
        {
            var userId = GetCurrentUserId();
            var diseases = await _context.Diseases
                .Include(d => d.Doctor)
                .Where(d => d.UserId == userId)
                .ToListAsync();

            return View(diseases);
        }

        // İlaçlar
        public async Task<IActionResult> MyDrugs()
        {
            var userId = GetCurrentUserId();
            var drugs = await _context.Drugs
                .Include(d => d.Doctor)
                .Where(d => d.UserId == userId)
                .ToListAsync();

            return View(drugs);
        }

        // Testler
        public async Task<IActionResult> MyTests()
        {
            var userId = GetCurrentUserId();
            var tests = await _context.Tests
                .Include(t => t.Doctor)
                .Where(t => t.UserId == userId)
                .ToListAsync();

            return View(tests);
        }

        // Randevular
        public async Task<IActionResult> MyAppointments()
        {
            var userId = GetCurrentUserId();
            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }

        // Yeni Randevu Oluştur
        public async Task<IActionResult> CreateAppointment()
        {
            var doctors = await _context.Users
                .Where(u => u.IsDoctor)
                .ToListAsync();

            ViewBag.Doctors = doctors;
            return View(new Appointment());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.UserId = GetCurrentUserId();
                appointment.Status = "Beklemede";
                
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyAppointments));
            }

            var doctors = await _context.Users
                .Where(u => u.IsDoctor)
                .ToListAsync();

            ViewBag.Doctors = doctors;
            return View(appointment);
        }

        // Randevu İptal
        [HttpPost]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == id && a.UserId == GetCurrentUserId());

            if (appointment != null && appointment.Status == "Beklemede")
            {
                appointment.Status = "İptal Edildi";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(MyAppointments));
        }
    }
} 