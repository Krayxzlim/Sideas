import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProfesionListComponent } from './profesion-list.component';

describe('ProfesionList', () => {
  let component: ProfesionListComponent;
  let fixture: ComponentFixture<ProfesionListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfesionListComponent], // Importa el componente standalone para testearlo
    }).compileComponents();

    fixture = TestBed.createComponent(ProfesionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    // Verifica que el componente se cree sin errores
    expect(component).toBeTruthy();
  });
});
