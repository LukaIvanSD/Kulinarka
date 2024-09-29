import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Comment } from '../model/recipeDetails';
import { CommentWithImage } from '../model/recipeDetails';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(private http:HttpClient) 
  { }
  GetComments(recipeId:number,pageNumber:number,pageSize:number):Observable<Comment[]>{
    let params= new HttpParams()
      .set('pageNumber',pageNumber.toString())
      .set('pageSize',pageSize.toString());
    return this.http.get<Comment[]>('https://localhost:7289/Comment/recipe/'+recipeId, {params});
  }  
  AddComment(comment :any):Observable<Comment>
  {
    return this.http.post<Comment>('https://localhost:7289/Comment',comment,{withCredentials:true});
  }
  AddExtendedComment(form :FormData):Observable<CommentWithImage>
  {
    return this.http.post<CommentWithImage>('https://localhost:7289/Comment/picture',form,{withCredentials:true});
  }
}
