import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardLight } from './dashboard-light';

describe('DashboardLight', () => {
  let component: DashboardLight;
  let fixture: ComponentFixture<DashboardLight>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardLight]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardLight);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
