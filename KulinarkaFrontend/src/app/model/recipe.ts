import { User } from './user';
export interface Recipe {
    id: number;
    userId: number;
    name: string;
    description: string;
    picture?:File;
    videoData?:Uint8Array;
    contentType?:string;
    duration: number;
    difficulty: string;
    numberOfPeople: number;
    chefsAdvice: string;
    creationDate: Date;
}
export interface RecipesHome{
    id:number;
    ownerUsername:string;
    name:string;
    pictureBase64:string;
    isPromoted:boolean;
}
export interface Ingredient{
    id:number;
    name:string;
    description?:string;
}
export interface PreparationStep{
    id:number;
    recipeId:number;
    description:string;
    order:number;
}
export interface Tag{
    id:number;
    recipeId:number;
    tagType:string;
    name:string;
}