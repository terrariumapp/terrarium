<%@ register tagprefix="controls" tagname="HeaderBar" src="~/Controls/HeaderBar.ascx" %>
<%@ register tagprefix="controls" tagname="InfoBar" src="~/Controls/InfoBar.ascx" %>
<%@ register tagprefix="controls" tagname="MenuBar" src="~/Controls/MenuBar.ascx" %>
<%@ Page Language="c#" %>

<HTML>
	<HEAD>
		<title></title>
		<link rel="stylesheet" type="text/css" href="/Terrarium/theme.css">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body background="/Terrarium/images/background.png">
		<!-- BEGIN CENTER ALIGNMENT TABLE -->
		<table border="0" cellpadding="0" cellspacing="0" width="100%" height="100%">
			<tr>
				<td align="center" valign="top">
					<!-- BEGIN MAIN LAYOUT TABLE -->
					<table border="0" cellpadding="0" cellspacing="4" width="100%" ID="Table1">
						<!-- BEGIN TITLE BAR AREA -->
						<tr>
							<td colspan="3" class="TitleBar">
								<controls:HeaderBar id="HeaderBar1" RunAt="server" />
							</td>
						</tr>
						<!-- END TITLE BAR AREA -->
						<tr>
							<!-- BEGIN LEFT MENU BAR -->
							<td class="MenuBar" align="center" valign="top" width="160">
								<controls:MenuBar id="menuBar" runat="server" />
							</td>
							<!-- END LEFT MENU BAR -->
							<!-- BEGIN CONTENT AREA -->
							<td width="*" valign="top">
								<table border="0" cellspacing="0" cellpadding="0" height="320" width="100%">
									<tr>
										<td class="MainBar">
											<H4>Graphics Engine and DirectX Whitepaper</H4>
											<h5>Overview</h5>
											<p>In any given Terrarium a lot of 
													action is happening.&nbsp; Every second there are approximately 10-20 frames of 
													animation, each with about 200 graphics draws required to construct the 
													frame.&nbsp; Since the Terrarium is graphics intensive and some time has to be 
													left for the actual engine to operate and run the creature code, DirectX was 
													chosen as the graphics API.&nbsp; DirectX allows for about 120 frames of 
													animation in the allotted on a modern day computer, while just sticking the 
													required performance on lower end laptops with a minimal amount of graphics 
													RAM.&nbsp; This made DirectX almost a requirement rather than a choice, since 
													GDI+ could have never delivered the required graphics performance on lower end 
													machines for the animations displayed.</p>
											<p>Since no managed implementation 
													exists for DirectX under .NET during the writing of the Terrarium application a 
													series of wrapper libraries had to be developed.&nbsp; At the first level a COM 
													Interop library was generated so that the Terrarium could interact with the DX7 
													VB Type Library.&nbsp; This was the easiest way to interact with DirectX since 
													generation of the type library was automatic using the TlbImp tool.&nbsp; To 
													further abstract Terrarium from DirectX a series of helper classes were used 
													and a form of Managed DirectX wrapper was generated to manage surfaces, 
													sprites, and transparency.&nbsp; This layer can be re-used in other 
													applications quite easily to create DirectX based games.</p>
											<p>Once the interop layer and wrapper 
													layer were completed it was time to actually implement the Terrarium game 
													view.&nbsp; The implementation of the Terrarium game view was built entirely on 
													the wrapper layer and contains mostly high level wrapper function calls and 
													very few low level DirectX calls.&nbsp; Some still exist, but creation of the 
													scene and managing DirectX were separated from one another where possible to 
													improve the clarity of the code (and in other places they were left alone to 
													improve performance).</p>
											<H5>DX 7 VB Type Library</h5>
											<p>Integration with DirectX is performed 
													using a custom type library that is wrapped around the DirectX 7 APIs.&nbsp; 
													This type library was traditionally used to enable DirectX from within Visual 
													Basic since it couldn’t easily consume the COM interfaces provided by DirectX 
													which is normally consumed using either C or C++.</p>
											<p>The benefits to the custom type 
													library are that many harder to use methods are wrapped in such a manner as to 
													make them much easier to use.&nbsp; For the purposes of the Terrarium the 
													primary methods revolve around the DirectDraw API all of which is easily 
													accessible through the use of some Interop classes created by the type library 
													importer.&nbsp; The following command line is used to generate the wrapper 
													library:</p>
											<p style="margin-left: 24px; font-family: courier new; font-size: 1.2em;">TlbImp dx7vb.dll /out:DxVBLib.dll 
													/namespace:DxVBLib /unsafe</p>
											<p>Once the type library is created it 
													can be included as a reference into any .NET application that needs to use the 
													functionality.&nbsp; In addition, the newly generated library is a managed 
													library so you can use ILDasm to investigate the method signatures available. 
													&nbsp;A namespace is provided so that the classes can easily be accessed with a 
													using statement.&nbsp; If no namespace is specified the TlbImp tool will 
													provide a default namespace anyway.</p>
											<H5>Managed DirectX Wrapper Library</H5>
											<p>To ease the development of a graphics 
													engine for the Terrarium gaming engine, it was decided that a managed DirectX 
													wrapper library could easily enable many of the common DirectDraw features 
													required without having to resort to direct Type Library calls every time a 
													graphics operation needs to be performed.</p>
											<p><h6>ManagedDirectX</h6></p>
											<p>The wrapper library developed for the 
													Terrarium consists of 5 primary classes.&nbsp; All top level DirectX 
													functionality is implemented by the <u>ManagedDirectX</u> class.&nbsp; This 
													class provides static access to the current <u>DirectX7</u> and <u>DirectDraw7</u>
													classes.&nbsp; Since these classes map back to COM interfaces they are cache 
													and only created once per application.&nbsp; For profiling the wrapper library, 
													the <u>ManagedDirectX</u> class also contains a static <u>Profiler</u> property 
													giving access to the <u>Profiler</u> class discussed in the Tools whitepaper.</p>
											<p><h6>DirectXException</h6></p>
											<p>If any errors occur when working with 
													the Type Library, a COMException is thrown.&nbsp; This COMException is then 
													mapped down to a <u>DirectXException</u> which can be caught by your 
													application.&nbsp; Often times when using various surface methods you’ll get an 
													exception if the surface has been reclaimed.&nbsp; If this occurs, then your 
													application can reinitialize the surfaces and continue.&nbsp; This is done 
													within the Terrarium graphics engine and is talked about later in this 
													whitepaper.</p>
											<p><h6>DirectDrawSurface</h6></p>
											<p>The <u>DirectDrawSurface</u> class is 
													used to wrap a native <u>DirectDrawSurface7</u> class which is implemented 
													within the Type Library.&nbsp; DirectX offers many surface related features 
													including loading static images into surfaces, performing transparency color 
													keying, and doing fast surface to surface transfers.&nbsp; Each surface is 
													constructed using a surface description class, <u>DDSURFACEDESC2</u>, located 
													inside of the Type Library.&nbsp; A default surface description might be 
													created as follows…</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
COLOR:green;
FONT-FAMILY:'Courier New'">// Setup default Surface Description</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">DefaultSurfaceDescription =</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>new</span> DDSURFACEDESC2();</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">DefaultSurfaceDescription.lFlags =</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; CONST_DDSURFACEDESCFLAGS.DDSD_CAPS;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">DefaultSurfaceDescription.ddsCaps.lCaps =</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN;</span></p>

											<p>The <u>DirectDrawSurface</u> class 
													implements several constructors to automatically generate surfaces without 
													having to worry about the underlying surface description properties, and 
													without having to understand the surface creation process.&nbsp; A surface can 
													be generated from a set of width, height properties, through the use of a 
													custom surface description structure, by the use of a file name that points to 
													an image the surface will be initialized with, or based on an existing <u>DirectDrawSurface7</u>
													class.&nbsp; These enable the quick and easy generation of nearly any type of 
													surface that might be used within most basic games.</p>
											<p>Once the surface has been 
													constructed, a call to <u>CreateSurface</u> is made.&nbsp; <u>CreateSurface</u> 
													contains all of the functionality for actually initializing the surface, since 
													the surface may need to be reinitialized after it is first constructed.&nbsp; 
													This happens when a surface needs to be restored after being lost.&nbsp; The 
													surface creation method examines the contents of all of the member fields and 
													decides whether to initialize a blank surface or an image surface.</p>
											<p>Once you have fully constructed and 
													initialized a new surface, you can check various properties of the surface as 
													necessary for your application.&nbsp; The <u>InVideo</u> property determines if 
													the surface was generated within system memory or within the cards graphics 
													memory.&nbsp; Video memory is much faster to work with than surface memory, but 
													is also highly limited in size (as low as only a couple of megs on some 
													laptops).&nbsp; The surface descriptor is made available through the <u>Descriptor</u>
													property, and for image based surfaces, this is how you would determine color 
													properties.&nbsp; The <u>Rect</u> property returns the size of the surface 
													(also important for image based surfaces since you won’t know the size until 
													after it has been loaded).&nbsp; The <u>Surface</u> property provides access to 
													the real <u>DirectDrawSurface7</u> object which will be heavily used later when 
													you’re calling surface methods.&nbsp; Surface blits could have been 
													encapsulated at this level, but it is quicker to provide direct access to the 
													real DirectDraw surface.&nbsp; The following code shows a basic surface 
													transfer using the <u>Surface</u> property.</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">RECT destRect = <span style='COLOR:blue'>new</span> RECT();</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">ManagedDirectX.DirectX.GetWindowRect(</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>this</span>.Handle.ToInt32(),</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>ref</span> destRect);</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">RECT srcRect = backBufferSurface.Rect;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">screenSurface.Surface.Blt(</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>ref</span> destRect, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; backBufferSurface.Surface, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>ref</span> srcRect, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; CONST_DDBLTFLAGS.DDBLT_WAIT);</span></p>
											<p>The only remaining feature of the <u>DirectDrawSurface</u>
													is the use of transparency keys.&nbsp; Transparency keys map specific colors so 
													that they are not copied during surface copies.&nbsp; Each surface can have a 
													color key applied.&nbsp; The <u>TransparencyKey</u> property can be set to a 
													valid <u>DDCOLORKEY</u> structure in which case the specified colors will be 
													marked as transparent.&nbsp; To more easily enable transparency several often 
													used color keys are pre-defined.&nbsp; <u>MagentaColorKey</u>, <u>WhiteColorKey</u>, 
													and <u>LimeColorKey</u> can all be passed directly to this property.&nbsp; In 
													addition a new color key can be generated using <u>GenerateColorKey</u> and 
													passing in the RGB value of the color to make transparent.&nbsp; Note that the 
													generated color value has to match the current color format and bit depth of 
													the surface the color key is being set upon.&nbsp; For this reason, the default 
													color key properties will almost always work, however, the <u>GenerateColorKey</u>
													method may not be 100% precise for every color.&nbsp; In this case a <u>DDCOLORKEY</u>
													will need to be generated by hand.&nbsp; The following code sets a default 
													transparency key for a surface, the default is currently set up as the <u>MagentaColorKey</u>
													in the code for <u>DirectDrawSurface</u>.<p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
COLOR:blue;
FONT-FAMILY:'Courier New'">this</span><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">.ddsurface.TransparencyKey = DirectDrawSurface.DefaultColorKey;</span></p>
											<p class="MsoNormal"><h6>DirectDrawSpriteSurface</h6></p>
											<p>The <u>DirectDrawSurface</u> is 
													useful for general purpose surfaces that are used as rendering buffers (like 
													the backBufferSurface and backgroundSurface in the Terrarium graphics engine) 
													and buffers that contain only a single sub-surface (the entire surface 
													represents a single image).&nbsp; This type of surface doesn’t contain 
													facilities for sprite animation.&nbsp; Sprite animations are stored in a single 
													surface, but only parts of the surface are rendered.&nbsp; Each part of the 
													surface contains a single frame of the animation in question.<p>
											<p>For working with sprite surfaces the <u>DirectDrawSpriteSurface</u>
													contains several useful methods.&nbsp; First, this surface encapsulates, rather 
													than inherits from <u>DirectDrawSurface</u>.&nbsp; There really isn’t any 
													special reason why encapsulation is used over inheritance, it was simply easier 
													to implement.&nbsp; Each sprite surface is constructed given a name for the 
													sprite surface, the image used to initialize the sprite surface, and the number 
													of frames the sheet should be broken into.&nbsp; A sheet can contain both rows 
													and columns of frames, so the number of frames in both the horizontal and 
													vertical direction is required.&nbsp; A perfect example is the sprite sheet for 
													creature animations which takes up 10 frames across and 40 frames down.&nbsp; 
													The following code is a slightly modified snippet out of the Terrarium graphics 
													engine that is used to load sprite surfaces.<p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">DirectDrawSpriteSurface dds =</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
COLOR:blue;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; new</span><span style="FONT-SIZE:
9pt;FONT-FAMILY:
'Courier New'"> DirectDrawSpriteSurface(</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Path.GetFileNameWithoutExtension(bmps[i]),</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; bmps[i],</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 10,</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 40</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; );</span></p>
											<p>Once your DirectDrawSpriteSurface is 
													created you have access to the height and width of each frame using the <u>FrameHeight</u>
													and <u>FrameWidth</u> properties respectively.&nbsp; This is dynamically 
													computed based on the number of frames defined in the constructor.&nbsp; You 
													can gain access to the underlying <u>DirectDrawSurface</u> using the <u>SpriteSurface</u>
													property.&nbsp; You’ll almost never need access to the underlying <u>DirectDrawSurface</u>
													since the sprite surface has many methods for grabbing and rendering frames.<p>
											<p>Three methods are available for 
													grabbing sprites.&nbsp; The basic <u>GrabSprite</u> method takes the frame 
													offset for the request frame.&nbsp; It then returns a bounding rectangle that 
													represents the surface location of the sprite.&nbsp; This method doesn’t do any 
													clipping.&nbsp; You’ll only want to use this method in very special 
													circumstances, since most of the time you’ll want to use the clipping methods.<p>
											<p>The remaining two <u>GrabSprite</u> methods 
													both support clipping.&nbsp; The first method takes a frame offset, as well as 
													a destination rectangle location, and the clipping bounds for the region the 
													sprite will be copied to.&nbsp; The destination rectangle is matched against 
													the clipping bounds region and a final set of source and destination rectangles 
													are created that can be used to transfer only the portion of the sprite that 
													will be available.&nbsp; The second method also takes a scaling factor.&nbsp; 
													This is used to compute a set of source and destination rectangles that can be 
													used to draw a scaled sprite.&nbsp; This second method isn’t currently used and 
													has some negative drawing artifacts.&nbsp; To be used in a real graphics engine 
													it will have to be updated significantly.&nbsp; Both of these methods return a <u>DirectDrawClippedRect</u>
													with the results of the sprite retrieval.&nbsp; The following code demonstrates 
													the use of the non scaling version of <u>GrabSprite</u>.&nbsp; Use of the <u>DirectDrawClippedRect</u>
													is discussed in the next section.<p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">DirectDrawClippedRect ddClipRect =</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; workSurface.GrabSprite(</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (<span style='COLOR:blue'>int</span>) 9, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ((<span style='COLOR:blue'>int</span>) DisplayAction.Died) + 
													framedir, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; dest, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; clipRect);</span></p>
											<p><h6>DirectDrawClippedRect</h6></p>
											<p>The <u>DirectDrawClippedRect</u> returns 
													information about a sprite retrieval.&nbsp; The returned structure can be used 
													to determine the source rectangle in the sprite sheet where the sprite can be 
													retrieved, the destination rectangle that the sprite should be written to 
													within the target surface, and whether or not the sprite has been clipped or is 
													invisible.&nbsp; <u>Destination</u> and <u>Source</u> are both basic <u>RECT</u>
													structures that can be passed directly to the <u>Blt</u> method on the <u>DirectDrawSurface7</u>.&nbsp; 
													The <u>Invisible</u> property determines if the sprite was completely clipped 
													and therefore invisible.&nbsp; A series of properties can be used to figure out 
													which of the edges of the sprite were clipped, <u>ClipTop</u>, <u>ClipLeft</u>, <u>ClipRight</u>, 
													and <u>ClipBottom</u>.&nbsp; The following code demonstrates the <u>DirectDrawClippedRect</u>
													being used in a <u>Blt</u> operation.<p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">dds.Surface.BltFast(</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; ddClipRect.Destination.Left,</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp; &nbsp;ddClipRect.Destination.Top,</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; workSurface.SpriteSurface.Surface,</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>ref</span> ddClipRect.Source,</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; CONST_DDBLTFASTFLAGS.DDBLTFAST_SRCCOLORKEY);</span></p>
											
											<p><h6>DirectDrawPictureBox</h6></p>
											<p>This gem derives from the <u>System.Windows.Forms.PictureBox</u>
													control and represents a drawing surface.&nbsp; By itself, it doesn’t do 
													anything, but it can be overridden by another class to implement your graphics 
													view.&nbsp; The <u>TerrariumDirectDrawGameView</u> derives from this class and 
													makes use of its protected properties to store vital screen information.&nbsp; 
													This base class overrides various Windows Forms painting methods so they don’t 
													perform their default painting behaviors, <u>OnPaint</u> and <u>OnPaintBackground</u>.&nbsp; 
													When in <u>DesignMode</u> the control will repaint its background to the 
													control’s <u>BackColor</u>.&nbsp; This is part of the normal process in 
													creating a control that supports both Windows Forms and DirectX.<p>
											
											<p>Overriding class should use the 
													properties on the new picture box control when they can.&nbsp; The <u>Clipper</u>
													field can be used to store a <u>DirectDrawClipper</u> for use in windowed 
													mode.&nbsp; The <u>screenSurface</u> field is used to store the primary screen 
													surface (for an implementation see the section on Game View Initialization”) 
													and the <u>backBufferSurface</u> field is used to store the primary work 
													surface for the application to implement simple double buffering.<p>

											<h5>World Construction – Height Fields and Optimizations</h5></p>
											
											<p>To provide a compelling visual 
													environment for the Terrarium a suitable and interesting backdrop needs to be 
													generated for the creatures to walk, eat, and fight upon.&nbsp; The environment 
													can’t be static for fear of becoming boring, and needs to be dynamically 
													created each time the Terrarium is loaded.&nbsp; It also needs to be capable of 
													providing a backdrop of varying size that is sized for the computer the game is 
													being run on.<p>
											
											<h6>HeightMap</h6></p>
											<p>To create such a world, the concept 
													of a height map is used.&nbsp; A height map is an array of points that 
													describes a 3 dimensional surface that resembles terrain.&nbsp; In the case of 
													the Terrarium height map the x and z points are equally spaced in such a manner 
													that a 2 dimensional array can represent the height field with each element 
													representing one xz point, and each value representing the height on the y axis 
													of that point.<p>
											
											<p>To compute the heights of all of the 
													points in the array a standard fractal based method is used.&nbsp; The method 
													is called diamond sub-division, and works by computing the average height of 4 
													points of a square.&nbsp; This value is then used to determine the point 
													representing the center of the square area, along with some random deviation to 
													provide a bumpy factor.&nbsp; This division method is first called against the 
													entire array.&nbsp; The upper and lower bounds of the array are considered the 
													corner points.&nbsp; Once the center point is computed, the method is called 
													recursively on increasingly smaller squares until the entire height map has 
													been filled with height values.&nbsp; The resulting array, if plotted, should 
													resemble real life terrain.<p>
											
											<p>The diamond subdivision method has a 
													single flaw.&nbsp; It only works on specifically sized arrays of points.&nbsp; 
													For the purposes of the Terrarium the arrays had to be of arbitrary size.&nbsp; 
													Once the subdivision method is completed, some points remain unset.&nbsp; These 
													points are searched for, and a height is computed using a nearest point 
													algorithm by searching the rest of the array for height values. This is 
													actually quite efficient at completing the height map.<p>
											
											<p>Another problem exists with the 
													diamond subdivision method.&nbsp; It doesn’t create very appealing terrain 
													unless it has been seeded.&nbsp; Only the first 4 points need to be seeded, but 
													the seeding can go all the way down to several iterations to make more 
													appealing terrain.&nbsp; In the case of the Terrarium, the corner points and 
													center point are first set to the middle point value of the height field.&nbsp; 
													The middle point value is computed by taking the average of the minimum and 
													maximum values allowed for the height field.&nbsp; In the case of Terrarium the 
													minimum is 0 and the maximum is 256.&nbsp; Two of the 5 points are then 
													selected at random.&nbsp; One of the points is given the average between the 
													mid value and minimum value or a low point, the second point is given the 
													average between the mid value and the maximum value or a height point.&nbsp; 
													This creates consistently appealing terrain features.<p>
											
											<p>The Terrarium doesn’t support 256 
													different types of terrain.&nbsp; It only supports two types of terrain, dirt 
													and grass.&nbsp; Once the height field is computed, the values have to be 
													normalized to the values 0 and 1.&nbsp; This is called clamping.&nbsp; Clamping 
													is done on a percentage area basis by adding counting the points below and 
													above a specific height level (this starts at 100).&nbsp; The percentage of 
													heights below the height level must fall within what is called the sea level 
													range (13-17% in the current Terrarium).&nbsp; The height level is modified 
													until this percentage is met.&nbsp; Once it is, all points below the height 
													level are set to 1, and all points above the height level are set to 0.&nbsp; 
													This can be modified to support many more different levels and types of 
													terrain.&nbsp; Your only limited by the amount of graphics resources you have 
													available.<p>
											
											<h6>WorldMap</h6></p>
											<p>Once the height map is computed it is 
													time for the <u>WorldMap</u> to go to work.&nbsp; The <u>WorldMap</u> turns the 
													normalized height map data into an actual background map.&nbsp; The first step 
													in this process is the <u>TileFilterPass</u>.&nbsp; This filter pass converts 
													each height map point into a <u>TileInfo</u> structure.&nbsp; This structure is 
													capable of mapping the point along with the surrounding conditions to one of 
													the map tile images that will be used when rendering to the screen.<p>
											
											<p>One the tiles have been filtered to 
													figure out which images should be used during rendering a <u>TileOffsetPass</u> 
													is run.&nbsp; This pass takes each tile and computes a pair of coordinates 
													where the tile begins.&nbsp; Since the view in the Terrarium is isometric this 
													can be a somewhat difficult task to get correct.&nbsp; The offsets are 
													extremely important and result in much faster background rendering if the 
													offsets don’t have to be computed at run-time.<p>
											
											<p>The final step in the world 
													generation process is the <u>MiniMapPass</u>.&nbsp; The <u>MiniMapPass</u> converts 
													the height field values in the height field map into a final bitmap.&nbsp; This 
													bitmap can be used to display a basic rendering of the overall set of terrain 
													features.&nbsp; The Terrarium uses this to allow for mini map navigation and to 
													show the user the region they are currently viewing.<p>
											
											<p>Once the map has been fully generated 
													you’ll use the <u>MiniMap</u> property to retrieve the bitmap based mini-map of 
													the final set of terrain.&nbsp; The <u>Map</u> property can be used to retrieve 
													an array of <u>TileInfo</u> structures that represent the world data.&nbsp; The <u>Map</u>
													property is used heavily at render time in order to get the tiles required to 
													render the background image.&nbsp; The following code demonstrates part of the 
													background rendering code that makes use of the <u>WorldMap</u> and several 
													properties.<p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">RECT dest;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">TileInfo tInfo = wld.Map[i,j];</span></p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
COLOR:green;
FONT-FAMILY:'Courier New'">/* Get world offset */</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">dest.Top = tInfo.YOffset;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">dest.Left = tInfo.XOffset;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
COLOR:green;
FONT-FAMILY:'Courier New'">/* Compute viewport offset */</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">dest.Top -= viewsize.Top;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">dest.Left -= viewsize.Left;</span></p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">dest.Bottom = dest.Top + wld.TileYSize;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">dest.Right = dest.Left + wld.TileXSize;</span></p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
COLOR:blue;
FONT-FAMILY:'Courier New'">int</span><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'"> iTrans =</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; (tInfo.Tile &lt; 8) ?</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; (tInfo.Transition*2) :</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; (tInfo.Transition*2) + 1;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
COLOR:blue;
FONT-FAMILY:'Courier New'">int</span><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'"> iTile = </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; (tInfo.Tile &lt; 8) ? </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; tInfo.Tile : </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; tInfo.Tile - 8;</span></p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">DirectDrawClippedRect ddClipRect = </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; workSurface.GrabSprite(iTile, iTrans, dest, clipRect);</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
COLOR:blue;
FONT-FAMILY:'Courier New'">if</span><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'"> (!ddClipRect.Invisible)</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">{</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; backgroundSurface.Surface.BltFast(</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ddClipRect.Destination.Left, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ddClipRect.Destination.Top, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; workSurface.SpriteSurface.Surface, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>ref</span> ddClipRect.Source, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; CONST_DDBLTFASTFLAGS.DDBLTFAST_WAIT | </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; CONST_DDBLTFASTFLAGS.DDBLTFAST_SRCCOLORKEY);</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">}</span></p>
											
											
											<h5>Resource Management – Resource 
														fallback and Surface management</h5></p>
											
											<p>The Terrarium is a game of resources, 
													primarily resources in the form of sprite sheets for creature animation.&nbsp; 
													Most of the resources needed by the Terrarium can be pre-loaded, items like the 
													cursors, background slides, and the default set of plants skins.&nbsp; However, 
													the Terrarium also supports custom skins, skins which can’t be known by the 
													Terrarium team when the application is compiled.&nbsp; This means some 
													resources will have to be loaded dynamically.&nbsp; Since portions of the 
													resources are loaded dynamically, even some of the default resources can be 
													loaded in the same way to conserve memory whenever the resources aren’t 
													needed.&nbsp; There are several classes to help implement the resource 
													management in the Terrarium.&nbsp; <u>TerrariumSpriteSurface</u> is capable of 
													working with sprites that require more than a single surface to be 
													loaded.&nbsp; It also handles returning closest match sheets whenever the 
													sprite is requested.&nbsp; 
													&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; The <u>TerrariumSpriteSurfaceManager</u>
													works to manage each of the loaded <u>TerrariumSpriteSurface</u> objects.&nbsp; 
													Whenever a specific sprite surface can’t be found, the sprite surface manager 
													attempts to load the necessary resources.&nbsp; To enable high speed text 
													within the Terrarium a text manager was also added to facilitate rendering text 
													to cached surfaces since surface transfer is faster than drawing new 
													text.&nbsp; The class for this is the <u>TerrariumTextSurfaceManager</u> and is 
													a subset of the functionality in the <u>TerrariumSpriteSurfaceManager</u>.<p>
											
											<h6>TerrariumSpriteSurface</h6></p>
											<p>The <u>TerrariumSpriteSurface</u> is 
													used to hold references to all of the surfaces used by a single sprite.&nbsp; 
													The Terrarium has the concept of sized surfaces.&nbsp; Each surface is a 
													different size, but represents graphics for the same sprite.&nbsp; This is used 
													to render graphics for creatures as they grow.&nbsp; Sprites can either have a 
													default surface set, in which case the surface is not sized, or a surface can 
													be added that is keyed to a specific size.&nbsp; Normally a sized surface will 
													have at least two different sizes represented in the sized surface collection.<p>
											
											<p>You should never have to work with 
													the <u>TerrariumSpriteSurface</u> class by hand since creation of these 
													surfaces is fully handled by the <u>TerrariumSpriteSurfaceManager</u> class.&nbsp; 
													This class can dynamically add any requested sprites that don’t already exist 
													within the surface manager, and it understands how to add both single sheet and 
													multi sheet surfaces.&nbsp; If you need low level support for adding surfaces 
													directly to the surface manager, then you can use this class directly.<p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">TerrariumSpriteSurface tss = <span style='COLOR:
blue'>new</span> TerrariumSpriteSurface();</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tss.AttachSurface(<span style='COLOR:blue'>new</span> DirectDrawSpriteSurface(key, bmppath, xFrames, 
													yFrames));</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">sprites[key] = tss;</span></p>
											
											<p><h6>TerrariumSpriteSurfaceManager</h6><p>
											<p>The <u>TerrariumSpriteSurfaceManager</u>
													is the major work-horse for the Terrarium application when it comes to surface 
													management.&nbsp; In general you should only have to add oddly shaped surfaces 
													to the surface manager manually. &nbsp;This includes the background slides, 
													cursors, and teleporter animations.&nbsp; The surface manager is capable of 
													loading both plant and animal surfaces on the fly as they are requested.&nbsp; 
													So for the most part, you’ll interact with the sprite surface manager using a 
													series of default indexed properties.<p>
											
											<p>One form of the indexed property 
													takes the key name of the sprite sheet you’re wishing to load.&nbsp; If the 
													surface manager has already loaded this sprite sheet then it is returned.&nbsp; 
													If it hasn’t, then it tries to add a sprite surface with the given name and a 
													set of 10x40 animations.&nbsp; This means the auto-load functionality only 
													works for the animal animations.&nbsp; This isn’t the main indexed property to 
													use though.<p>
											
											<p>The second indexed property takes the 
													key name of the sprite sheet you want to use, the ideal frame size you are 
													looking for, and whether the sprite sheet should be loaded as an animal sprite 
													sheet.&nbsp; If the surface has already been loaded, then the <u>GetClosestSurface</u>
													of the <u>TerrariumSpriteSurface</u> is called to get the closest matching 
													sprite surface.&nbsp; Else, the sprite surface has to be loaded.&nbsp; If the 
													animal property is true then a new sprite surface with a set of 10x40 
													animations is loaded, else a plant is loaded with a set of 1x1 
													animations.&nbsp; The sprite surface manager is extremely convenient since it 
													is accessed just like a Hashtable using the sprite’s key name as the look-up 
													value.&nbsp; Not only is it simple to retrieve a sprite surface, it also 
													handles automatic loading of most sprite surfaces.<p>
											
											<p>Some sprite surfaces do have to be 
													loaded manually.&nbsp; Two methods exist for loading sprite surfaces, the first 
													for single sheet surfaces, and the second for multi sheet surfaces.&nbsp; The <u>Add</u>
													method takes a sprite’s key name, and the number of frames and animations 
													available for the sprite.&nbsp; The method for adding sprite sheets is fairly 
													intense and involves several checks to make sure the right sprite sheets are 
													being loaded for a given sprite key.&nbsp; The following pseudo code 
													demonstrates the lookup path.<p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;FONT-FAMILY:'Courier New'">Do any 
													bitmaps exist on disk that match the key?</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;FONT-FAMILY:'Courier New'">&nbsp; Yes</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; 
													You’re done, fall through and try to load</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;FONT-FAMILY:'Courier New'">&nbsp; No</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; 
													Sprites don’t exist for this item.</span></p>
											<p>Once everything is complete the 
													bitmaps are again scanned for on the disk.&nbsp; If they do exist after the 
													previous process they are loaded and attached to a <u>TerrariumSpriteSurface</u>.&nbsp; 
													Since this was the <u>Add</u> method, only a single surface should be 
													attached.&nbsp; The <u>AddSizedSurface</u> can be used to add a surface that 
													has multiple surfaces to attach.&nbsp; The algorithms are the same.&nbsp; So 
													that sums up the surface manager.&nbsp; The following code shows how to add a 
													couple of specialized surfaces.<p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tsm.Remove("background");</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tsm.Add("background", 8, 4);</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'"></span>&nbsp;</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tsm.Remove("backgroundGrid");</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tsm.Add("backgroundGrid", 8, 4);</span></p>
											
											<h6>TerrariumTextSurfaceManager</h6></p>
											<p>The text surface manager was made as 
													a performance improvement to the Terrarium graphics engine.&nbsp; During the 
													initial performance testing phase it was found that drawing text was more than 
													twice as slow as copying between surfaces.&nbsp; The text surface manager is 
													capable of drawing text onto a surface, and then returning that surface so that 
													it can be transferred to the final image.&nbsp; Since the text is only drawn 
													once, and then cached, drawing text becomes much more performant since multiple 
													calls to <u>DrawText</u> aren’t being made.<p>
											
											<p>The text surface manager works using 
													the same principles as the sprite surface manager.&nbsp; A single indexed 
													property takes the text to be rendered as the argument and returns a surface 
													with the text rendered on it.&nbsp; The surface is only created the first time 
													a new piece of text is requested, and then returned from the cache each time 
													thereafter.&nbsp; The following code uses the text surface manager to render 
													some text to the screen.<p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">DirectDrawSurface textSurface = tfm[((Species) orgState.Species).Name];</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
COLOR:blue;
FONT-FAMILY:'Courier New'">if</span><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'"> (textSurface != <span style='COLOR:blue'>null</span> &amp;&amp; textSurface.Surface != <span style='COLOR:blue'>
														null</span>)</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">{</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; dds.Surface.BltFast(</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ddClipRect.Destination.Left, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ddClipRect.Destination.Top - 14, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; textSurface.Surface, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>ref</span> TerrariumTextSurfaceManager.StandardFontRect,
												</span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; CONST_DDBLTFASTFLAGS.DDBLTFAST_SRCCOLORKEY);</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">}</span></p>
											<h5>Game View Initialization</h5></p>
											
											<p>In order for the Terrarium graphics 
													engine to also be a Windows Forms control it can’t just be initialized on 
													construction.&nbsp; Doing so would mean that DirectX would initialize during <u>DesignMode</u>
													or that special code would have to be created to prevent such an 
													occurrence.&nbsp;&nbsp; Rather than bother with this work-around, the graphics 
													engine can be initialized by whenever it is needed.&nbsp; This can be while the 
													game is starting up, or after it has run for many hours.&nbsp; The <u>InitializeDirectDraw</u>
													method is the key to getting everything started.&nbsp; Once called this method 
													initializes the graphics engine to run in windowed mode, or full screen (not 
													fully implemented).&nbsp; It is responsible for the creation of all of the 
													initial surfaces, including the primary surface and the 
													backBufferSurface.&nbsp; And in windowed mode it sets up a clipper object so 
													that only the game view window is used as a rendering surface.<p>
											
											<p>To bring up DirectX and DirectDraw 
													the application needs to set a cooperative level.&nbsp; For this to work, there 
													has to be an owning window somewhere (even in full screen mode) because a 
													handle is needed.&nbsp; In windowed mode, which is the only mode fully 
													supported by the Terrarium graphics engine, no additional parameters are needed 
													and the cooperative level is set to normal.&nbsp; This means that the graphics 
													engine shares the graphics card with other applications and DirectX and GDI run 
													side-by-side.&nbsp; An odd side-effect that was found during testing was that 
													the form must be visible before passing its handle to the <u>SetCooperativeLevel</u>
													method, so make sure you perform this step.<p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
COLOR:blue;
FONT-FAMILY:'Courier New'">this</span><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">.Parent.Show();</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">ManagedDirectX.DirectDraw.SetCooperativeLevel(</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>this</span>.Parent.Handle.ToInt32(), </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; CONST_DDSCLFLAGS.DDSCL_NORMAL);</span></p>
											
											<p>The remainder of the code for 
													bringing up the surfaces is in the <u>CreateWindowedSurfaces</u>.&nbsp; The 
													code was broken out so that it could also be called to reinitialize surfaces 
													once the device was lost (this will be covered later).&nbsp; The code begins by 
													setting up a set of surface description flags to create a primary 
													surface.&nbsp; The <u>DirectDrawSurface</u> doesn’t have a surface descriptor 
													for this step, but you can easily make one by hand.<p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">DDSURFACEDESC2 tempDescr = </span><span style='COLOR:blue'>new</span> DDSURFACEDESC2();<p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tempDescr.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tempDescr.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_PRIMARYSURFACE;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">screenSurface = <span style='COLOR:blue'>new</span> DirectDrawSurface(tempDescr);</span></p>
											
											<p>With the primary surface in place, 
													your application is ready to draw.&nbsp; However, if you start drawing directly 
													to the screen you’ll get tearing and screen flicker because graphics calls 
													might bridge vertical retraces on the screen.&nbsp; Needless to say this is 
													bad, and should be avoided.&nbsp; At this point you can also overwrite any 
													portion of the screen you’d like.&nbsp; DirectX doesn’t have automatic clipping 
													protection, but you can set up a clipper for your windows region.&nbsp; The 
													process is to create an empty clipper, then attach the clipper to your window 
													using the window handle.&nbsp; The clipper will automatically compute the 
													appropriate bounding rectangle for the window.&nbsp; You’ll then attach the 
													clipper to the primary surface.&nbsp; Note that DirectDraw will clip any draw 
													that exceeds the bounds of the clipper in such a way that nothing will be 
													rendered.&nbsp; If something is outside of the range of the clipper the draw 
													simply gets thrown out.&nbsp; That is why clipping was also implemented earlier 
													in the sprite surface.</p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">Clipper = ManagedDirectX.DirectDraw.CreateClipper(0);</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">Clipper.SetHWnd(<span style='COLOR:blue'>this</span>.Handle.ToInt32());</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">screenSurface.Surface.SetClipper(Clipper);</span></p>
											
											<p>For the Terrarium graphics engine, 
													three more surfaces are created.&nbsp; A basic backBufferSurface is used to 
													implement double buffering, even though we have more than two buffers, only 
													double buffering is used because of the way the other two buffers are set 
													up.&nbsp; Another buffer is used to cache the background so that it doesn’t 
													have to be repainted every frame, this is the <u>backgroundSurface</u>.&nbsp; 
													This is important, since the time to draw the background is actually quite 
													lengthy and can damage frame rates on slow cards.&nbsp; Oddly enough rendering 
													30 small grid square regions is also much slower than rendering one large 
													one.&nbsp; So each frame the background is rendered from the background surface 
													to the staging surface (actually once every 10 frames).&nbsp; The only 
													exception is when the background is changing, which occurs when the user is 
													scrolling or otherwise moving the viewport area.&nbsp; Once the background 
													changes the background surface is again built up, causing the frame rate to 
													drop a bit, until the next frame where the cached background is used again.<p>
											
											<p>The staging surface is present for 
													another optimization.&nbsp; Plants only need to be rendered once every 10 
													frames as well.&nbsp; They don’t support animation and are static images just 
													like the background.&nbsp; For this reason, another caching surface was 
													created.&nbsp; This will be covered more in the render loop, but the basic 
													premise is that every ten frames the background surface gets rendered to the 
													staging surface, and then the graphics engine renders all of the plants to the 
													staging surface.&nbsp; Each frame thereafter, the staging surface is 
													transferred directly to the back buffer skipping quite a bit of painting steps, 
													and then any creature animations are rendered.&nbsp; The results are finally 
													sent to the primary surface and displayed to the user.&nbsp; The following code 
													demonstrates the creation of the back buffer surface.&nbsp; The other two 
													surfaces share the same creation code and are created as plain off-screen 
													surfaces.&nbsp; Once created they can either exist in video memory or surface 
													memory.&nbsp; Video memory surfaces will operate much faster and provide a 
													great performance increase over system memory surfaces.<p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tempDescr = <span style='COLOR:blue'>new</span> DDSURFACEDESC2();</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tempDescr.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS | </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; CONST_DDSURFACEDESCFLAGS.DDSD_HEIGHT | </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; CONST_DDSURFACEDESCFLAGS.DDSD_WIDTH;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tempDescr.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tempDescr.lWidth = <span style='COLOR:blue'>this</span>.Width;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tempDescr.lHeight = <span style='COLOR:blue'>this</span>.Height;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">backBufferSurface = <span style='COLOR:blue'>new</span> DirectDrawSurface(tempDescr);</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">Trace.WriteLine("Back Buffer Surface InVideo? " + backBufferSurface.InVideo);</span></p>
											
											<p>The final initialization steps 
													involve resetting the terrarium (this simply creates a new <u>WorldMap</u> and 
													gets it ready for when the scene needs to be rendered), and clearing the three 
													back surfaces.&nbsp; This step isn’t important if you’re updating the entire 
													surface every frame (since anything previously there will be overwritten), but 
													it is better to be safe than sorry.<p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">ResetTerrarium();</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">ClearBackground();</span></p>
											
											<p>With the initialization of DirectX 
													done you’re ready to set up any initial textures in the surface manager.&nbsp; 
													The Terrarium generally needs at least the cursors and background slides to be 
													initialized since they don’t have a standard format and will be loaded 
													improperly by the automatic texture loader.&nbsp; The graphics engine has a 
													simple method <u>AddBackgroundSlide</u> that automatically adds any background 
													slides.&nbsp; This means you can only have a single set of backgrounds, but 
													that isn’t generally an issue (this could be updated for more complex 
													background scenarios).&nbsp; You’ll use the <u>AddComplexSpriteSurface</u> method 
													to add the cursor and teleporter sprite sheets.&nbsp; This method allows you to 
													directly add a sprite that doesn’t have a standard set of frame 
													dimensions.&nbsp; Finally you’ll use the <u>AddComplexSizedSpriteSurface</u> to 
													add any special animated textures or textures linked to either animals or 
													plants.&nbsp; The Terrarium pre-caches the plant skin since it is the most 
													often used skin.<p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tddGameView.AddBackgroundSlide();</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tddGameView.AddComplexSpriteSurface("cursor", 1, 9);</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tddGameView.AddComplexSpriteSurface("teleporter", 10, 1);</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">tddGameView.AddComplexSizedSpriteSurface("plant", 1, 1);</span></p>

											<h5>Game View Render Loop</h5>
											
											<p>Once the graphics engine is fully 
													initialized it is ready to go.&nbsp; The graphics engine is fully encapsulated 
													and only needs to have a single method called to start rendering the background 
													area into the game view window.&nbsp; The <u>RenderFrame</u> method should be 
													called whenever you want to paint a frame.&nbsp; In the current Terrarium this 
													should be called 20 times per second.&nbsp; This is the frame rate the graphics 
													engine is prepared to run at, and it won’t run correctly at any other speed 
													(animations will be choppy).<p>
											
											<p>To enable the display of creatures 
													and animations within the game view, a world vector should be set.&nbsp; A 
													world vector is a special state object from the Terrarium game engine that 
													identifies where each creature is currently located and where it is 
													going.&nbsp; The graphics engine uses this information to compute frame offsets 
													for each of the creatures to achieve movement and animation.&nbsp; This world 
													vector should be set every 10 frames and the Terrarium is keyed to expect the 
													object every 10 frames (only 10 frames worth of movement are 
													interpolated).&nbsp; If the <u>UpdateWorld</u> method isn’t called with a new 
													vector, then creatures will appear to move endlessly along their original path, 
													never stopping.<p>
											
											<p>So what happens on a call to <u>RenderFrame</u>?&nbsp; 
													First all surfaces are checked to make sure they are still valid (at least the 
													primary surfaces are checked, the surfaces in the surface manager are left 
													alone).&nbsp; If any of them have been lost because of a device reset, then the 
													entire Terrarium graphics engine is reset.&nbsp; You can tell if a surface has 
													been lost using the <u>isLost</u> method on the underlying direct draw 
													surface.&nbsp; If the surfaces have been lost then all of the sprite surfaces 
													within the surface manager need to be re-added.&nbsp; You’ll notice the code 
													for this looks almost identical to the initialization that occurred when 
													originally constructing the game view.&nbsp; The background will also need to 
													be repainted.&nbsp; Device loss is a terrible thing and can put most DirectX 
													applications on their knees if they are properly coded to manage surface loss.<p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
COLOR:blue;
FONT-FAMILY:'Courier New'">if</span><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'"> (screenSurface.Surface.isLost() != 0 || </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; backBufferSurface.Surface.isLost() != 0)</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">{</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; ReInitSurfaces();</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>this</span>.AddBackgroundSlide();</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>this</span>.AddComplexSpriteSurface("cursor", 1, 9);</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>this</span>.AddComplexSpriteSurface("teleporter", 10, 1);</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>this</span>.AddComplexSizedSpriteSurface("plant", 1, 1);</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>this</span>.viewchanged = <span style='COLOR:blue'>true</span>;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">}</span></p>
											
											<p>The render loop then goes into draw 
													mode.&nbsp; The process is to paint the background, paint the sprites to the 
													back buffer, paint any game messages using the text rendering engine, and 
													finally paint the cursor.&nbsp; The order is important since everything is 
													overlaid and you don’t want your cursor behind the creatures or behind your 
													messages.&nbsp; Once the back buffer surface has been updated with the latest 
													scene it is transferred to the primary screen.<p>
											
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">PaintBackground();</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">PaintSprites(backBufferSurface, <span style='COLOR:blue'>false</span>);</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">PaintMessage();</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">PaintCursor();</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'"></span>&nbsp;</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">RECT destRect = <span style='COLOR:blue'>new</span> RECT();</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">ManagedDirectX.DirectX.GetWindowRect(</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>this</span>.Handle.ToInt32(),</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>ref</span> destRect);</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'"></span>&nbsp;</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">RECT srcRect = backBufferSurface.Rect;</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">screenSurface.Surface.Blt(</span></p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>ref</span> destRect, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; backBufferSurface.Surface, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; <span style='COLOR:blue'>ref</span> srcRect, </span>
											</p>
											<p class="MsoNormal"><span style="FONT-SIZE:9pt;
FONT-FAMILY:'Courier New'">&nbsp;&nbsp;&nbsp; CONST_DDBLTFLAGS.DDBLT_WAIT);</span></p>
											
											<h6>PaintBackground</h6>
											<p>The <u>PaintBackground</u> method is 
													responsible for providing Terrarium with an Isometric game view.&nbsp; An 
													Isometric game view uses diamond shaped land tiles rather than square based 
													land tiles to create a form of 3D tiling effect.&nbsp; In order for and 
													Isometric view to work, the tiles have to be offset so that they combine 
													properly.&nbsp; The following image shows the background images used for the 
													Terrarium, you’ll notice that the actual tiles are square with transparent 
													regions, but the visible part is a diamond.&nbsp; These diamonds connect with 
													one another to make a continuous land body.<p>
											
											<p><img width="504" height="126" src="Terrarium%20Graphics%20Engine%20and%20DirectX%20Whitepaper_files/image001.jpg"><p>
											<p>The problem with an Isometric layout 
													is that the final map actually has to be square.&nbsp; Since the viewport is 
													square the map has to map into that area.&nbsp; This means several of the tiles 
													being drawn will be drawn outside of the bounds of the viewport, they will be 
													drawn halfway off the screen.&nbsp; This leads to an equation that can help us 
													draw if the location of the first diamond in the upper left of the viewport can 
													be placed.&nbsp; The diamond is placed according to the following diagram at 
													location –(1/2 width), -(1/2 height).&nbsp; Height and width are governed 
													separately since most Isometric game tiles are twice as long as they are tall 
													giving a better perspective once the tiles are combined.<p>
											
											<p><img width="450" height="300" src="Terrarium%20Graphics%20Engine%20and%20DirectX%20Whitepaper_files/image002.gif"><p>
											
											<p>The previous diagram also shows 
													another feature.&nbsp; You’ll notice that every odd numbered column is offset 
													upwards along the y-axis.&nbsp; Each column is then offset along the x-axis by 
													only one half of the width of a tile.&nbsp; This means the width of each tile 
													is 64, but each tile is rendered at (tileIndex * (64 / 2)) – (64 / 2), rather 
													than being offset in a square fashion and being (tileIndex * 64) – (64 / 
													2).&nbsp; The isometric layout discussed here is fairly standard.&nbsp; Some 
													games may have slightly modified version to handle different perspectives, but 
													for the most part that is how the background is generated.<p>
											
											<p>If you examine the diagram that 
													contains the map tiles you’ll see how they are created in order to make sure 
													the images tile correctly without seams.&nbsp; These were produced by a 
													professional graphics artist, but if you need to create additional images for 
													your own game, they aren’t difficult to generate with a little time and effort.<p>
											
											<h6>PaintSprites</h6>
											<p>The <u>PaintSprites</u> method is 
													called in two different manners.&nbsp; The first application of the method is 
													used by the <u>PaintBackground</u> method to notify the sprite engine that it 
													needs to render any static (non-animated) images to the staging surface.&nbsp; 
													This is important because it enables more complex scenes with many plants and 
													animals to be rendered in a much more efficient manner since the savings on 
													rendering static images is 90% (normally you’d render 10 * plants, for ten 
													frames, but only 1 * plants is required, saving you 9 * plants, or 90% of your 
													rendering time).<p>
											
											<p>The process for rendering any type of 
													sprite involves the basic steps of selecting graphics resources, determining a 
													clipping region, and finally transferring the sprite to the render 
													surface.&nbsp; This is actually easier than you might think.<p>
											
											<p>The process of selecting the 
													appropriate media is handled by the surface manager.&nbsp; The surface manager 
													is queried for the custom sprite key associated with the animal or plant in 
													question.&nbsp; Since the selection process takes into account size and whether 
													the type is plant or animal by default, no extra processing is required by the 
													painting methods.&nbsp; If the sprite key can’t be found in the surface 
													manager, the sprite key for the creature’s skin family is searched for.&nbsp; 
													The Skin family sprites ship with the Terrarium as part of the game and should 
													always be available.&nbsp; Custom skins are generally only available on a few 
													machines, so they will be loaded if available, but fallback to the skin family 
													almost always occurs.&nbsp; If the skin family can’t be found then the 
													Terrarium falls back to the Ant or Plant skin.&nbsp; If these don’t exist then 
													you get an invisible or non-rendered creature.&nbsp; You really don’t want this 
													to happen, which is why so many fallback methods were implemented.<p>
											
											<p>Once the media is selected the frame 
													offset has to be determined.&nbsp; The Terrarium sprite sheet for animation is 
													laid out using 40 rows and 10 columns.&nbsp; The 40 rows encode 8 directions 1 
													per row for 8 rows, and 5 actions for a total of 40 rows.&nbsp; Properties of 
													the organism are investigated and the action and direction are used to offset 
													index the sprite sheet and select the appropriate row to index into.&nbsp; Once 
													the row is indexed the column is indexed using the current frame number 
													offset.&nbsp; That’s all you need to grab a sprite.<p>
											
											<p>The basic <u>GrabSprite</u> with 
													clipping methods are used to actually get the render source and target.&nbsp; 
													This step always happens even if the creature might not be visible (you can 
													optimize this out using a better Z-Order Occlusion algorithm).&nbsp; With the 
													sprite information in hand you have enough to transfer the sprite to the 
													surface.&nbsp; If the creature is not <u>Invisible</u> then the <u>BltFast</u> methods 
													on the target surface are called to transfer the image with full clipping 
													thanks to the <u>GrabSprite</u> method.<p>
											
											<p>Some additional surface methods are 
													used to render other cool features.&nbsp; The text is rendered if the organism 
													type is that of an animal.&nbsp; Destination lines, bounding boxes, and 
													selection boxes are also drawn if they are enabled.&nbsp; All of these features 
													are just implemented using line and rectangle drawing methods on the underlying 
													surface object.&nbsp; These are provided by the VB Type Library since 
													DirectDraw doesn’t have equivalent methods.&nbsp; They could definitely be made 
													faster using custom graphics and <u>Blt</u> calls rather than surface locking 
													and direct manipulation routines (that is what the <u>DrawLine</u> and <u>DrawBox</u>
													methods do behind the scenes).<p>
											
											<h6>PaintMessage</span></h6>
											<p>Since the Terrarium already has 
													Windows Forms UI it is generally very easy to get a message to the user.&nbsp; 
													Normally messages come in the form of MessageBox calls, or they are placed 
													within one of the Trace windows available for the user to look at.&nbsp; 
													However, some messages are best left to interrupt the game view.&nbsp; The <u>PaintMessage</u>
													method uses the <u>DrawText</u> method on the back buffer surface in order to 
													render a centered and new lined wrapped message for the user.&nbsp; This method 
													can be used to notify the user of critical conditions.<p>
											
											<p>One problem with the use of a message 
													drawing API at the game view level is that it doesn’t work unless you 
													continuously make calls to <u>RenderFrame</u>.&nbsp; For truly critical 
													failures you’ll most likely have stopped the game engine loop and drawing loops 
													so your message won’t be seen.&nbsp; You’ll need to use another method of 
													displaying user messages when this happens.<p>
											
											<h6>PaintCursor</h6>
											<p>Early on a default windows cursor was 
													used for the Terrarium, but the cursor is one of the primary methods of 
													interacting with a user or notifying the users of items they can interact 
													with.&nbsp; Currently the Terrarium supports two different types of cursors, 
													the default or selection cursor, and the scrolling cursors.&nbsp; 8 different 
													cursors are available for scrolling in all 8 directions along the edge of the 
													Terrarium.<p>
											
											<p>To implement custom cursors, whenever 
													the game view is active, the default Windows Forms cursor is turned off.&nbsp; 
													The mouse events are trapped so that the graphics engine knows where the 
													current cursor should be displayed and to determine which cursor needs to be 
													shown.&nbsp; Cursors are not animated, but easily could be.&nbsp; There could 
													also be different cursors implemented for holding the mouse buttons down and 
													using any sort of drag features.&nbsp; This is one of the easier places to add 
													features to the existing graphics engine.<p>
											
											<h5>Secondary Game View Features</h5>
											
											<p>The game view is not only responsible 
													for rendering the animation portion of the Terrarium game, but also for 
													providing navigation features, access to the world data for mini-map 
													generation, and information about creature clicks.&nbsp; The game view is a 
													fully feature component which can be dragged and dropped onto a new Windows 
													Form.&nbsp; Once dropped into place, it has designable properties for setting 
													rendering features, rendering textual messages, and several special event 
													notifications which can be used to enable custom UI for better user 
													interaction.<p>
											
											<p>First the Terrarium enables full 
													mini-map support and navigation features for use with the mini-map.&nbsp; The 
													UI can be scrolled by leaving the mouse over the edges of the game view or 
													through the use of the scrolling API.&nbsp; The <u>ScrollUp</u>, <u>ScrollDown</u>,
													<u>ScrollLeft</u> and <u>ScrollRight</u> methods can each be used to scroll the 
													game view by a fixed amount in any direction.&nbsp; The <u>CenterTo</u> method 
													will attempt to center the game view to a specific point within the game 
													world.&nbsp; A special mini-map control can be used to intercept mouse clicks 
													and turn the clicked point into world coordinates to move the game view around 
													the world using the scroll APIs.<p>
											
											<p>The normal mini-map generated by the <u>World</u>
													class isn’t the only item provided by the Terrarium graphics engine.&nbsp; In 
													addition to the background image representing a scaled down view of the actual 
													backdrop, the graphics engine also renders the creature locations as points 
													onto the bitmap.&nbsp; This additional UI feature can be used to show the user 
													where creatures are so they don’t have to examine the entire map looking for 
													them.&nbsp; This is especially useful for finding unique items like the 
													teleporter.<p>
											
											<p>Since the game view does change over 
													time and creatures do move, the Terrarium graphics engine supplies an event to 
													notify the client of mini-map updates.&nbsp; The MiniMapUpdated event is fired 
													whenever a new mini-map is made available and the new bitmap can be used to 
													update any UI that was displaying the previous bitmap.<p>
											
											<p>The Terrarium graphics engine can 
													also be used to notify the client of creature selection.&nbsp; Clicks on the 
													game view can be mapped into creature space to see if the click intersects with 
													any creatures.&nbsp; When a click does intersect with a creature the creature 
													becomes selected.&nbsp; The client is notified of this through the <u>OrganismClicked</u>
													event and can enable any features that should be available for clicked 
													creatures.&nbsp; The Terrarium graphics engine performs its own processing and 
													sets the selected flag for any clicked creatures and then draws a selection 
													rectangle around the creature to alert the user.<p>
										</td>
									</tr>
								</table>
							</td>
							<!-- END CONTENT AREA -->
							<!-- BEGIN RIGHT MENU BAR -->
							<td class="MenuBar" align="center" valign="top">
								<controls:InfoBar id="InfoBar1" runat="server" />
							</td>
							<!-- END RIGHT MENU BAR -->
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<!-- END CENTER ALIGNMENT TABLE -->
	</body>
</HTML>
