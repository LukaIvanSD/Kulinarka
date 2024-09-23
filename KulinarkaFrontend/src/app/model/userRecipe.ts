import { Tag } from "./recipe";
export interface Ingredient{
    name:string,
    amount:string,
    measurementType:string
}
export interface Recipe{
    id:number,
    name:string 
    difficulty:string,
    pictureBase64 :string,
    duration:number,
    numberOfPeople:number,
    creationDate:Date
}
export interface UserRecipe{
    recipe : Recipe,
    ingredients:Ingredient[],
    tags:Tag[]
    isPromoted:boolean
}