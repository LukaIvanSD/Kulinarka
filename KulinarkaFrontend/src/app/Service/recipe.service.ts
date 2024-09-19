import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RecipesHome } from '../model/recipe';
import { RecipeDetails } from '../model/recipe';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {

  constructor(private http:HttpClient) { }
  GetSortedRecipes() {
     return this.http.get<RecipesHome[]>('https://localhost:7289/Recipes/sorted');
  }
  GetRecipeById(id:number){
    return this.http.get<RecipeDetails>('https://localhost:7289/Recipes/'+id);
  }
  AddRecipe(recipeForm:FormData){
    return this.http.post('https://localhost:7289/Recipes',recipeForm,{withCredentials:true});
  }
}
