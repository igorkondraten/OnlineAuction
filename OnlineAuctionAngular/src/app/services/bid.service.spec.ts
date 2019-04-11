import { TestBed } from '@angular/core/testing';

import { BidService } from './bid.service';

describe('BidService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BidService = TestBed.get(BidService);
    expect(service).toBeTruthy();
  });
});
