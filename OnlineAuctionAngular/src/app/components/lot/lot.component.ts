import { Component, OnInit } from '@angular/core';
import { Lot } from 'src/app/models/lot.model';
import { LotService } from 'src/app/services/lot.service';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { NgbModal, ModalDismissReasons, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BidService } from 'src/app/services/bid.service';
import { Bid } from 'src/app/models/bid.model';
import { AlertService } from 'src/app/services/alert.service';
import { AuthService } from 'src/app/services/auth.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-lot',
  templateUrl: './lot.component.html',
  styleUrls: ['./lot.component.css']
})
export class LotComponent implements OnInit {
  public lot: Lot;
  bidForm: FormGroup;
  modalReference: any;

  constructor(private lotService: LotService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private modalService: NgbModal,
    private alertService: AlertService,
    private bidService: BidService,
    private authService: AuthService) {
    this.bidForm = this.formBuilder.group({
      price: ['', [Validators.required, Validators.min(0.01)]]
    });
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.lotService.get(+params['id']).subscribe(lot => this.lot = lot);
    });
  }

  openModal(content) {
    this.bidForm.controls["price"].setValidators([Validators.required, Validators.min(this.lot.currentPrice + 0.01)]);
    this.modalReference = this.modalService.open(content);
    this.modalReference.result.then((result) =>
      this.bidService.create(this.lot.lotId, <Bid>{ price: result }).subscribe(
        () => {
          this.alertService.success("Bid added");
          this.ngOnInit();
        },
        (error) => this.alertService.error(error.error.message))
    );
  }

  public get isUserAdmin$(): Observable<boolean> {
    return this.authService.isAdmin();
  }

  public get isUserSeller$(): Observable<boolean> {
    return this.authService.isSeller();
  }

  public get isSignedIn$(): Observable<boolean> {
    return this.authService.isSignedIn();
  }

  deleteBid(bidId: number) {
    this.bidService.delete(bidId).subscribe(
      () => {
        this.lot.bids = this.lot.bids.filter(bid => bid.bidId !== bidId);
        this.alertService.success("Bid deleted successfully.");
      },
      (err) => this.alertService.error(err.error.error)
    )
  }

  onSubmit() {
    if (this.bidForm.invalid) {
      return;
    }
    this.modalReference.close(this.bidForm.controls.price.value);
  }
}
