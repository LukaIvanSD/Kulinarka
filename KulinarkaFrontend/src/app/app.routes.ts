import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { StatusComponent } from './common/status/status.component';
import { RecipeDetailsComponent } from './common/recipe-details/recipe-details.component';
import { LoginComponent } from './common/login/login.component';
import { authGuard } from './Guard/auth.guard';
import { HomeComponent } from './common/home/home.component';
import { RegisterComponent } from './common/register/register.component';
import { ProfileComponent } from './common/profile/profile.component';
import { AddRecipeComponent } from './common/add-recipe/add-recipe.component';
import { UserRecipesComponent } from './common/user-recipes/user-recipes.component';

export const routes: Routes = [
    {
        path: '',component: HomeComponent
    },
    {
        path:'recipeDetails/:id',loadComponent:()=>import("./common/recipe-details/recipe-details.component").then(r=>r.RecipeDetailsComponent),canActivate:[authGuard]
    },
    {
        path:'login',loadComponent:()=>import("./common/login/login.component").then(r=>r.LoginComponent)
    },
    {
        path:'register',loadComponent:()=>import("./common/register/register.component").then(r=>r.RegisterComponent)
    },
    {
        path:'profile',loadComponent:()=>import("./common/profile/profile.component").then(r=>r.ProfileComponent)
    },
    {
        path:'addRecipe',loadComponent:()=>import("./common/add-recipe/add-recipe.component").then(r=>r.AddRecipeComponent)
    },
    {
        path:'myRecipes',loadComponent:()=>import('./common/user-recipes/user-recipes.component').then(r=>r.UserRecipesComponent)
    },
    {
        path:'**' ,component:StatusComponent
    }

];
