import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfesionService, Profesion } from '../../services/profesion.service';

@Component({
  selector: 'app-profesion-list', // Selector para usar este componente en plantillas
  standalone: true, // Componente independiente (no en NgModule)
  templateUrl: './profesion-list.component.html',
  styleUrls: ['./profesion-list.scss'],
  imports: [CommonModule], // Módulos que usa (ngIf, ngFor, etc)
})
export class ProfesionListComponent {
  profesiones: Profesion[] = []; // Array con todas las profesiones traídas del backend
  profesionesFiltradas: Profesion[] = []; // Array con profesiones filtradas según texto ingresado
  filtro: string = ''; // Texto usado para filtrar profesiones

  constructor(private profesionService: ProfesionService) {}

  ngOnInit() {
    // Al iniciar, consulta al servicio las profesiones y luego filtra para mostrar todas inicialmente
    this.profesionService.getProfesiones().subscribe((data) => {
      this.profesiones = data;
      this.filtrarProfesiones();
    });
  }

  onFiltroChange(event: Event) {
    // Captura el cambio en el input y actualiza el filtro para luego filtrar la lista
    const input = event.target as HTMLInputElement | null;
    this.filtro = input?.value ?? '';
    this.filtrarProfesiones();
  }

  filtrarProfesiones() {
    // Convierte el filtro a minúsculas para hacer comparación case-insensitive
    const filtroMinuscula = this.filtro.toLowerCase();

    // Filtra las profesiones por descripción que incluya el texto ingresado
    this.profesionesFiltradas = this.profesiones.filter((profesion) =>
      profesion.descripcion.toLowerCase().includes(filtroMinuscula)
    );
  }
}
