import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FeedService {

  private readonly url : string = environment.api;
  constructor(private http : HttpClient) { }

  getFeeds() : Observable<any[]> {
    return this.http.get<any[]>(`${this.url}feed`);
  }
}
