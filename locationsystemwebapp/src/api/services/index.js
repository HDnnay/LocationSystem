// Export all API services from a single file for easy access
import appointmentService from './appointmentService'
import dentistService from './dentistService'
import patientService from './patientService'
import dentalOfficeService from './dentalOfficeService'
import permissionService from './permissionService'

export {
  appointmentService,
  dentistService,
  patientService,
  dentalOfficeService,
  permissionService
}
