import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Recipe, RecipesHome } from '../model/recipe';
import { RecipeDetails } from '../model/recipe';
import { Observable } from 'rxjs';
import { UserRecipe } from '../model/userRecipe';

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
  GetUserRecipes():Observable<UserRecipe[]>{
    return this.http.get<UserRecipe[]>('https://localhost:7289/Recipes/user',{withCredentials:true});
  }
  DeleteRecipe(recipeId:number):Observable<Recipe>{
    return this.http.delete<Recipe>('https://localhost:7289/Recipes/'+recipeId,{withCredentials:true})
  }
  PromoteRecipe(recipeId:number){
    return this.http.post( 'https://localhost:7289/PromotionRewardRecipe/'+recipeId, {}, { withCredentials: true });
  }
}
