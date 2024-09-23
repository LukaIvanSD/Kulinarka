import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-recipe-details',
  standalone: true,
  imports: [ReactiveFormsModule,FormsModule,CommonModule],
  templateUrl: './recipe-details.component.html',
  styleUrl: './recipe-details.component.css'
})
export class RecipeDetailsComponent {
  recipe: any; 
  isEditModalOpen = false;

  constructor() {
  }

  OpenEditModal(): void {
    this.isEditModalOpen = true;
  }

  CloseEditModal(): void {
    this.isEditModalOpen = false;
  }

  SaveChanges(): void {
    console.log('Promene sačuvane:', this.recipe);
    this.CloseEditModal();
  }
}
