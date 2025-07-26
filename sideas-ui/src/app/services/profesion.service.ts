import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// Interfaz que representa la estructura de una profesión recibida
export interface Profesion {
  id: number;
  profesionCodigo: number;
  especialidad: number;
  descripcion: string;
  estado: string;
}

@Injectable({
  providedIn: 'root', // Servicio singleton disponible en toda la app
})
export class ProfesionService {
  private apiUrl = 'http://localhost:5190/api/profesiones'; // URL base del endpoint REST

  constructor(private http: HttpClient) {}

  // Método para obtener el listado completo de profesiones desde el backend
  getProfesiones(): Observable<Profesion[]> {
    return this.http.get<Profesion[]>(this.apiUrl);
  }
}
