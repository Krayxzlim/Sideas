import { TestBed } from '@angular/core/testing';
import { ProfesionService } from './profesion.service';
//Test unitario
describe('Profesion', () => {
  let service: ProfesionService;

  beforeEach(() => {
    TestBed.configureTestingModule({}); // Configura el módulo de pruebas
    service = TestBed.inject(ProfesionService); // Inyecta el servicio para testear
  });

  it('should be created', () => {
    // Verifica que el servicio se cree correctamente
    expect(service).toBeTruthy();
  });
});
