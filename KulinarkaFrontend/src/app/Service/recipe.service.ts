import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Recipe, RecipesHome } from '../model/recipe';
import { PreparedRecipeImage, RecipeDetails } from '../model/recipeDetails';
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
  GetRecipeDetails(recipeId:number):Observable<RecipeDetails>{
    return this.http.get<RecipeDetails>("https://localhost:7289/RecipeDetails/"+recipeId);
  }
  IsUserOwnerOfRecipe(recipeId:number):Observable<boolean>{
    return this.http.get<boolean>("https://localhost:7289/Recipes/IsUserOwnerOfRecipe/"+recipeId,{withCredentials:true});
  }
  UpdateRecipe(recipeDetails:RecipeDetails){
    console.log(recipeDetails);
    return this.http.patch('https://localhost:7289/RecipeDetails/',recipeDetails,{withCredentials:true});
  }
  GetPreparedRecipeImages(recipeId:number,pageNumber:number,pageSize:Number):Observable<PreparedRecipeImage[]>{
    let params = new HttpParams().set('pageNumber',pageNumber.toString()).set('pageSize',pageSize.toString());
    return this.http.get<PreparedRecipeImage[]>('https://localhost:7289/PreparedRecipeImage/'+recipeId,{params});
  }
  UploadPreparedRecipeImage(form:FormData):Observable<PreparedRecipeImage>{
    return this.http.post<PreparedRecipeImage>('https://localhost:7289/PreparedRecipeImage/',form,{withCredentials:true});
  }
  HasUserUploadedImage(recipeId:number):Observable<boolean>{
    return this.http.get<boolean>('https://localhost:7289/PreparedRecipeImage/check/'+recipeId,{withCredentials:true});
  }
}
