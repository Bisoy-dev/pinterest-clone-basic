import { Component, OnInit } from '@angular/core';
import { FeedService } from './services/feed.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  feeds : any[] = []

  constructor(private feedService : FeedService){}
  ngOnInit(): void {
    this.loadFeeds()
  }

  loadFeeds() : void {
    this.feedService.getFeeds()
      .subscribe(data => {
        this.feeds = data;
        console.log(this.feeds)
      })
  }
}
