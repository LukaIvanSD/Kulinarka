import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Tag } from '../model/tag';

@Injectable({
  providedIn: 'root'
})
export class TagService {

  constructor(private http:HttpClient) { }
  GetTags(){
    return this.http.get<Tag[]>('https://localhost:7289/Tag');
  }
}
