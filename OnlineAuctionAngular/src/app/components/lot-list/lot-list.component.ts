import { Component, OnInit, Output } from '@angular/core';
import { LotService } from 'src/app/services/lot.service';
import { Lot } from 'src/app/models/lot.model';
import { AuthService } from 'src/app/services/auth.service';
import { Observable } from 'rxjs';
import { RouterModule } from '@angular/router';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-lot-list',
  templateUrl: './lot-list.component.html',
  styleUrls: ['./lot-list.component.css']
})
export class LotListComponent implements OnInit {
  limit: number = 10;
  offset: number = 0;
  page: number = 1;
  lots: Lot[];
  totalCount: number;
  searchQuery: string;

  constructor(private lotService: LotService, 
    private authService: AuthService, 
    private router: RouterModule,
    private alertService: AlertService) { }

  ngOnInit() {
    this.getLots();
  }

  getLots(){
    this.lotService.getAll(this.limit, this.offset, this.searchQuery).subscribe(
      (lotsList) => {         
        if (!lotsList){
          this.alertService.error("Lots not found.");
          this.lots = null;
          return;
        }          
        this.lots = lotsList.lots;
        this.totalCount = lotsList.totalCount;
      },
      (e) => console.log(e));
  }

  onPageChanged(page: number){
    window.scroll(0,0);
    this.offset = (this.limit * page) - this.limit;
    this.getLots();
  }

  changeLimit(limit: number){
    window.scroll(0,0);
    this.limit = limit;
    this.offset = 0;
    this.page = 1;
    this.getLots();
  }

  search(query: string){
    if (!query)
      return;
    this.searchQuery = query;
    this.getLots();
  }

  resetSearch(searchBox){
    this.searchQuery = null;
    searchBox.value = null;
    this.getLots();
  }

  deleteLot(lotId: number){
    this.lotService.delete(lotId).subscribe(() => {
      this.lots = this.lots.filter(lot => lot.lotId !== lotId);
      this.alertService.success("Lot deleted successfully.");
      window.scroll(0,0);
    },
    (err) => this.alertService.error(err.error.error));
  }

  public get isUserSignedIn$(): Observable<boolean> {
    return this.authService.isSignedIn();
  }

  public get isUserAdmin$(): Observable<boolean> {    
    return this.authService.isAdmin();
  }
}
