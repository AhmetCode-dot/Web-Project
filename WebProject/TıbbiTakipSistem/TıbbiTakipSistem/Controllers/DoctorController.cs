using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TıbbiTakipSistem.Data;
using TıbbiTakipSistem.Models;
using System.Security.Claims;

namespace TıbbiTakipSistem.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        }

        // Hastalarım
        public async Task<IActionResult> Patients()
        {
            var doctorId = GetCurrentUserId();
            var patients = await _context.Users
                .Where(u => !u.IsDoctor && 
                    (_context.Appointments.Any(a => a.DoctorId == doctorId && a.UserId == u.UserId) ||
                     _context.Diseases.Any(d => d.DoctorId == doctorId && d.UserId == u.UserId) ||
                     _context.Drugs.Any(d => d.DoctorId == doctorId && d.UserId == u.UserId) ||
                     _context.Tests.Any(t => t.DoctorId == doctorId && t.UserId == u.UserId)))
                .Distinct()
                .ToListAsync();

            return View(patients);
        }

        // Hasta Detayı
        public async Task<IActionResult> PatientDetails(int id)
        {
            var doctorId = GetCurrentUserId();
            var patient = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id && !u.IsDoctor);

            if (patient == null)
                return NotFound();

            var diseases = await _context.Diseases
                .Where(d => d.UserId == id && d.DoctorId == doctorId)
                .ToListAsync();

            var drugs = await _context.Drugs
                .Where(d => d.UserId == id && d.DoctorId == doctorId)
                .ToListAsync();

            var tests = await _context.Tests
                .Where(t => t.UserId == id && t.DoctorId == doctorId)
                .ToListAsync();

            ViewBag.Diseases = diseases;
            ViewBag.Drugs = drugs;
            ViewBag.Tests = tests;

            return View(patient);
        }

        // Randevular
        public async Task<IActionResult> Appointments()
        {
            var doctorId = GetCurrentUserId();
            var appointments = await _context.Appointments
                .Include(a => a.User)
                .Where(a => a.DoctorId == doctorId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }

        // Randevu Onayla/Reddet
        [HttpPost]
        public async Task<IActionResult> UpdateAppointment(int id, string status)
        {
            var doctorId = GetCurrentUserId();
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == id && a.DoctorId == doctorId);

            if (appointment != null && appointment.Status == "Beklemede")
            {
                appointment.Status = status;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Appointments));
        }

        // Hastalık Ekle
        [HttpPost]
        public async Task<IActionResult> AddDisease(Disease disease)
        {
            if (ModelState.IsValid)
            {
                disease.DoctorId = GetCurrentUserId();
                disease.DiagnosisDate = DateTime.Now;
                
                _context.Diseases.Add(disease);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(PatientDetails), new { id = disease.UserId });
        }

        // İlaç Ekle
        [HttpPost]
        public async Task<IActionResult> AddDrug(Drug drug)
        {
            if (ModelState.IsValid)
            {
                drug.DoctorId = GetCurrentUserId();
                drug.StartDate = DateTime.Now;
                
                _context.Drugs.Add(drug);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(PatientDetails), new { id = drug.UserId });
        }

        // Test Ekle
        [HttpPost]
        public async Task<IActionResult> AddTest(Test test)
        {
            if (ModelState.IsValid)
            {
                test.DoctorId = GetCurrentUserId();
                test.TestDate = DateTime.Now;
                
                _context.Tests.Add(test);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(PatientDetails), new { id = test.UserId });
        }
    }
} 