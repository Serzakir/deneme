function ref_hiz = Ref_hiz( mesafe,zaman )

% initialize variables
 NUM_SECONDS_PER_DAY = 86400.0;
% convert times to fractional days using datenum
 timeFractionalDays = datenum(zaman);
% leave only the part with the most recent day fraction
 timeDayFraction = mod(timeFractionalDays,1);
% multiply by number of seconds in a day
 timeInSeconds = timeDayFraction .* NUM_SECONDS_PER_DAY;

  
toplam=400;
bitis_suresi=20;
ref_hiz=(toplam-mesafe)*1000/(bitis_suresi*3600-timeInSeconds)*3.6;



end

