import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfesionListComponent } from './components/profesion-list/profesion-list.component';
// Importa y muestra lista de profesiones
@Component({
  selector: 'app-root', // Selector raíz de la app
  standalone: true, // Componente standalone
  templateUrl: './app.component.html',
  imports: [CommonModule, ProfesionListComponent], // Importa componentes y módulos usados
})
export class AppComponent {
  title = 'sideas-ui'; // Título de la aplicación (puede usarse en la vista)
}
