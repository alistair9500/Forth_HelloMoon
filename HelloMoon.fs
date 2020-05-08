: hmHeader ( -- )
	page ."  -- Moon Lander -- "
	cr
	cr ." YOU ARE LANDING ON THE MOON AND AND HAVE TAKEN OVER MANUAL" 
	cr ." CONTROL 1000 FEET ABOVE A GOOD LANDING SPOT. YOU HAVE A DOWN-" 
	cr ." WARD VELOCITY OF 50 FEET/SEC. 150 UNITS OF FUEL REMAIN." 
	cr
	cr ." HERE ARE THE RULES THAT GOVERN YOUR APOLLO SPACE-CRAFT:" 
	cr
	cr ." (1) AFTER EACH SECOND THE HEIGHT, VELOCITY, AND REMAINING FUEL"
	cr ."     WILL BE REPORTED VIA DIGBY YOUR ON-BOARD COMPUTER."
	cr ." (2) AFTER THE REPORT A '?' WILL APPEAR. ENTER THE NUMBER"
	cr ."     OF UNITS OF FUEL YOU WISH TO BURN DURING THE NEXT"
	cr ."     SECOND. EACH UNIT OF FUEL WILL SLOW YOUR DESCENT BY"
	cr ."     1 FOOT/SEC."
	cr ." (3) THE MAXIMUM THRUST OF YOUR ENGINE IS 30 FEET/SEC/SEC"
	cr ."     OR 30 UNITS OF FUEL PER SECOND."
	cr ." (4) WHEN YOU CONTACT THE LUNAR SURFACE. YOUR DESCENT ENGINE"
	cr ."     WILL AUTOMATICALLY SHUT DOWN AND YOU WILL BE GIVEN A"
	cr ."     REPORT OF YOUR LANDING SPEED AND REMAINING FUEL."
	cr ." (5) IF YOU RUN OUT OF FUEL THE '?' WILL NO LONGER APPEAR"
	cr ."     BUT YOUR SECOND BY SECOND REPORT WILL CONTINUE UNTIL"
	cr ."     YOU CONTACT THE LUNAR SURFACE." 
	cr
	cr ." BEGINNING LANDING PROCEDURE........" 
	cr ." G O O D  L U C K ! ! !" 
	cr
	cr ." SEC   FEET SPEED FUEL PLOT OF DISTANCE" 
	cr ;

Variable FuelBurn
Variable Fuel
Variable VelBeforeBurn
Variable VelAfterBurn
fVariable Height
Variable Time
fVariable ActualTimeToContact
	
: hmVariables ( -- )
	0 FuelBurn !
	150 Fuel !
	50 VelBeforeBurn !
	1000  s>f  Height f!
	0 Time !
	0 VelAfterBurn !
	0 s>f ActualTimeToContact f!
;
	
: hmShow ( -- )
	Time @ 3
	.r space 
	Height f@ 6 1 0
	f.rdp space
	VelBeforeBurn @ 5
	.r space 
	Fuel @ 4
	.r
	cr
;

: hmGetBurn ( -- )
    Fuel @ 0 >
	if
	  pad 2 accept
	  pad swap s>number?
	  drop d>s
	  FuelBurn !
	  cr
	 else
	   0 FuelBurn !
	 endif
;

: hmCalc ( -- )
	FuelBurn @ 0 <
	if 
		0 FuelBurn !
	endif
	FuelBurn @ 30 >
	if
		30 FuelBurn !
	endif
	FuelBurn @ Fuel @ >
	if
		Fuel @ FuelBurn !
	endif
	Fuel @ FuelBurn @ - 
	Fuel !
	VelBeforeBurn @ FuelBurn @ 5 - - 
	VelAfterBurn !
	Height f@ VelBeforeBurn @ VelAfterBurn @ + s>f  f2/  f- 
	Height f!
	Time @ 1 + Time !
	VelAfterBurn @ VelBeforeBurn !
;

: hmLanding ( -- )
	Height f@ VelBeforeBurn @ VelAfterBurn @ + s>f f2/ f+ 
	Height f!
	FuelBurn @ 5 =
	if
		Height @ s>f VelBeforeBurn @ s>f f/
		ActualTimeToContact f!
	else
		VelBeforeBurn @ dup * s>f
		10 FuelBurn @ 2 * -  s>f 
		Height f@ f* f+
		fsqrt
		VelBeforeBurn @ s>f f-
		5 FuelBurn @ - s>f f/
		ActualTimeToContact f!
	endif
	." Touchdown at "
	Time @ 1 - s>f ActualTimeToContact f@ f+
	6 2 0
	f.rdp space ." seconds"
	cr
	." Landing Velocity = "
	VelBeforeBurn @ s>f 
	5 FuelBurn @ - s>f
	ActualTimeToContact f@ f* f+
	6 2 0 f.rdp
	space ." Feet/Sec"
	cr
	Fuel @ 3 .r space ." Units of Fuel remaining"
	cr
	VelAfterBurn @ 0=
	if
		cr ." CONGRATULATIONS! A PERFECT LANDING!!"
		cr ." YOUR LICENSE WILL BE RENEWED  ....  LATER."
	endif
	VelAfterBurn @ abs 2 <
	if
		cr ." Any Landing you can walk away from is a good landing"
\		cr ." YOUR LICENSE WILL BE RENEWED  ....  LATER."
	else
		cr ." ***** SORRY, BUT YOU BLEW IT!!!!"
		cr ." APPROPRIATE CONDOLENCES WILL BE SENT TO YOUR NEXT OF KIN" 
	endif
;

: HelloMoon ( -- )
	hmHeader 
	hmVariables
	hmShow
	BEGIN
	  hmGetBurn
	  hmCalc
	  hmShow
	  Height f@ f0<=
	 UNTIL
	 hmLanding
;