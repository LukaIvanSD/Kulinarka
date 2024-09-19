import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RecipeService } from '../../Service/recipe.service';
import { RecipesHome } from '../../model/recipe';
import { Router } from '@angular/router';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  recipes: RecipesHome[] = [];
  
  constructor(private recipeService:RecipeService,private router:Router) { }

  ngOnInit(): void {
    this.recipeService.GetSortedRecipes().subscribe({
      next: (data) => {
        this.recipes = data;
        console.log('Recipes:', this.recipes);
      },
      error: (err) => {
        console.error('Error getting recipes:', err);
      }
    });
  }
  ShowRecipeDetails(recipeId: number) {
    this.router.navigate(['/recipeDetails', recipeId]);
  }

}

