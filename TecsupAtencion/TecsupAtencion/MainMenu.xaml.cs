using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;
using Coding4Fun.Kinect.Wpf.Controls;
using System.Windows.Ink;
using System.Windows.Media.Animation;
using Microsoft.Samples.Kinect.WpfViewers;




namespace TecsupAtencion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainMenu : UserControl, ISwitchable
    {
        public string stringhb = "menu";
        public Boolean flecha = false;
        private static double _topBoundary;
        private static double _bottomBoundary;
        private static double _leftBoundary;
        private static double _rightBoundary;
        private static double _itemLeft;
        private static double _itemTop;
        bool closing = false;
        const int skeletonCount = 6;
        Skeleton[] allSkeletons = new Skeleton[skeletonCount];

        public MainMenu()
        {
            InitializeComponent();

            #region HoverButton direccionar

            #region menu HoverButton
            menuHbCursosProgramacionExtension.Click += new RoutedEventHandler(menuHbCursosProgramacionExtension_Click);
            menuHbCarrerasProfesionales.Click += new RoutedEventHandler(menuHbCarrerasProfesionales_Click);
            menuHbConsultoriaInvestigacionAplicada.Click += new RoutedEventHandler(menuHbConsultoriaInvestigacionAplicada_Click);
            menuHbContactenos.Click += new RoutedEventHandler(menuHbContactenos_Click);
            #endregion

            #region cpe HoverButton
            cpeHbAdministracionLinux.Click += new RoutedEventHandler(cpeHbAdministracionLinux_click);
            cpeHbComunicacionesInalambricas.Click += new RoutedEventHandler(cpeHbComunicacionesInalambricas_click);
            cpeHbTelefoniaIpAsterisk.Click += new RoutedEventHandler(cpeHbTelefoniaIpAsterisk_click);
            cpeHbVozTelefoniaIp.Click += new RoutedEventHandler(cpeHbVozTelefoniaIp_click);
            #endregion

            #region flecha HoverButton

            flechaHbIzquierda.Click += new RoutedEventHandler(flechaHbIzquierda_click);
            flechaHbDerecha.Click += new RoutedEventHandler(flechaHbIzquierda_click);

            #endregion

            #endregion
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.cpe.Visibility = Visibility.Hidden;
            this.flechaDerecha.Visibility = Visibility.Hidden;
            this.flechaIzquierda.Visibility = Visibility.Hidden;

            kinectSensorChooser1.KinectSensorChanged += new DependencyPropertyChangedEventHandler(kinectSensorChooser1_KinectSensorChanged);
            
            KinectSensor sensor = KinectSensor.KinectSensors[0];
            KinectSensorManager manager = new KinectSensorManager();
            manager.KinectSensor = sensor;
            kinectColorViewer1.KinectSensorManager = manager;
        }

        #region HoverButton acciones
        #region menu HoverButton

        void menuHbCursosProgramacionExtension_Click(object sender, RoutedEventArgs e)
        {
            this.stringhb = "cpe";
            this.cpe.Visibility = Visibility.Visible;
            this.menu.Visibility = Visibility.Hidden;
            this.flecha = true;
            this.flechaIzquierda.Visibility = Visibility.Visible;
            this.flechaDerecha.Visibility = Visibility.Visible;
        }

        void menuHbCarrerasProfesionales_Click(object sender, RoutedEventArgs e)
        {
            
            
        }

        void menuHbConsultoriaInvestigacionAplicada_Click(object sender, RoutedEventArgs e)
        {

        }

        void menuHbContactenos_Click(object sender, RoutedEventArgs e)
        {
            

        }
        #endregion

        #region cpe HoverButton

        void cpeHbAdministracionLinux_click(object sender, RoutedEventArgs e)
        {
        }

        void cpeHbComunicacionesInalambricas_click(object sender, RoutedEventArgs e)
        {    
        }

        void cpeHbTelefoniaIpAsterisk_click(object sender, RoutedEventArgs e)
        {
            
        }

        void cpeHbVozTelefoniaIp_click(object sender, RoutedEventArgs e)
        {
            this.strtunghb = "vti";
            this.cpe.Visibility = Visibility.Hidden;
            this.vti.Visibility = Visibility.Visible;
        }
        #endregion

        #region flecha HoverButton

        void flechaHbIzquierda_click(object sender, RoutedEventArgs e)
        {
            if (stringhb.Equals("cpe"))
            {
                this.stringhb = "menu";
                this.cpe.Visibility = Visibility.Hidden;
                this.menu.Visibility = Visibility.Visible;
                this.flecha = false;
                this.flechaIzquierda.Visibility = Visibility.Hidden;
                this.flechaDerecha.Visibility = Visibility.Hidden;

            }

        }

        void flechaHbDerecha_click(object sender, RoutedEventArgs e)
        {
        }
        #endregion


        #endregion


        void kinectSensorChooser1_KinectSensorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            KinectSensor old = (KinectSensor)e.OldValue;

            StopKinect(old);

            KinectSensor sensor = (KinectSensor)e.NewValue;

            if (sensor == null)
            {
                return;
            }

            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.3f,
                Correction = 0.0f,
                Prediction = 0.0f,
                JitterRadius = 1.0f,
                MaxDeviationRadius = 0.5f
            };
            sensor.SkeletonStream.Enable(parameters);

            sensor.SkeletonStream.Enable();

            
            sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs> (sensor_AllFramesReady);
            sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

            try
            {
               
                sensor.Start();
            }
            catch (System.IO.IOException)
            {
                kinectSensorChooser1.AppConflictOccurred();
            }
        }

        void sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e )
        {
            if (closing)
            {
                return;
            }

            //Get a skeleton
            Skeleton first = GetFirstSkeleton(e);

            if (first == null)
            {
                return;
            }

            GetCameraPoint(first, e);

            //set scaled position
            //ScalePosition(headImage, first.Joints[JointType.Head]);
            //ScalePosition(leftEllipse, first.Joints[JointType.HandLeft]);
            ScalePosition(RightHand, first.Joints[JointType.HandRight]);
            ScalePosition(LeftHand, first.Joints[JointType.HandLeft]);
        }

        void GetCameraPoint(Skeleton first, AllFramesReadyEventArgs e)
        {

            using (DepthImageFrame depth = e.OpenDepthImageFrame())
            {
                if (depth == null ||
                    kinectSensorChooser1.Kinect == null)
                {
                    return;
                }

                //Map a joint location to a point on the depth map
                //head
                DepthImagePoint headDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.Head].Position);
                //left hand
                DepthImagePoint leftDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.HandLeft].Position);
                //right hand
                DepthImagePoint rightDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.HandRight].Position);


                //Map a depth point to a point on the color image
                //head
                ColorImagePoint headColorPoint =
                    depth.MapToColorImagePoint(headDepthPoint.X, headDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left hand
                ColorImagePoint leftColorPoint =
                    depth.MapToColorImagePoint(leftDepthPoint.X, leftDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //right hand
                ColorImagePoint rightColorPoint =
                    depth.MapToColorImagePoint(rightDepthPoint.X, rightDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);


                //Set location
                //CameraPosition(headImage, headColorPoint);
                //CameraPosition(leftEllipse, leftColorPoint);
                CameraPosition(RightHand, rightColorPoint);
                CameraPosition(LeftHand, rightColorPoint);

                #region HoverButton convertir en Check

                #region menu HoverButton

                if (stringhb.Equals("menu"))
                {
                    CheckButton(menuHbCursosProgramacionExtension, RightHand);
                    CheckButton(menuHbCarrerasProfesionales, RightHand);
                    CheckButton(menuHbConsultoriaInvestigacionAplicada, RightHand);
                    CheckButton(menuHbContactenos, RightHand);
                }
                #endregion

                #region cpe HoverButton
                if (stringhb.Equals("cpe"))
                {
                    CheckButton(cpeHbComunicacionesInalambricas, RightHand);
                    CheckButton(cpeHbAdministracionLinux, RightHand);
                    CheckButton(cpeHbTelefoniaIpAsterisk, RightHand);
                    CheckButton(cpeHbVozTelefoniaIp, RightHand);
                }
                #endregion

                #region flecha HoverButton
                if (flecha == true)
                {
                    CheckButton(flechaHbDerecha, RightHand);
                    CheckButton(flechaHbIzquierda, RightHand);
                }

                #endregion

                #endregion

            }
        }


        Skeleton GetFirstSkeleton(AllFramesReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null)
                {
                    return null;
                }


                skeletonFrameData.CopySkeletonDataTo(allSkeletons);

                //get the first tracked skeleton
                Skeleton first = (from s in allSkeletons
                                  where s.TrackingState == SkeletonTrackingState.Tracked
                                  select s).FirstOrDefault();

                return first;



            }
        }


        private void btnangle_Click(object sender, RoutedEventArgs e)
        {
            if (kinectSensorChooser1.Kinect.ElevationAngle != (int)slider1.Value)
            {
                kinectSensorChooser1.Kinect.ElevationAngle = (int)slider1.Value;
            }

            //if (runtime.NuiCamera.ElevationAngle != (int)slider1.Value)
            //{
            //    runtime.NuiCamera.ElevationAngle = (int)slider1.Value;
            //}

        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int n = (int)slider1.Value;

            n = n + 27;
            n = (n * 100) / 54;

            Degree.Content = n.ToString() + "%";

        }

        private void StopKinect(KinectSensor sensor)
        {
            if (sensor != null)
            {
                if (sensor.IsRunning)
                {
                    //stop sensor 
                    sensor.Stop();

                    //stop audio if not null
                    if (sensor.AudioSource != null)
                    {
                        sensor.AudioSource.Stop();
                    }


                }
            }
        }

        private void CameraPosition(FrameworkElement element, ColorImagePoint point)
        {
            //Divide by 2 for width and height so point is right in the middle 
            // instead of in top/left corner
            Canvas.SetLeft(element, point.X - element.Width / 2);
            Canvas.SetTop(element, point.Y - element.Height / 2);

        }

        private static void CheckButton(HoverButton button, Ellipse thumbStick)
        {

            if (IsItemMidpointInContainer(button, thumbStick))
            {
                button.Hovering();
            }
            else
            {
                button.Release();
            }
        }


        public static bool IsItemMidpointInContainer(FrameworkElement container, FrameworkElement target)
        {
            FindValues(container, target);

            if (_itemTop < _topBoundary || _bottomBoundary < _itemTop)
            {
                //Midpoint of target is outside of top or bottom
                return false;
            }

            if (_itemLeft < _leftBoundary || _rightBoundary < _itemLeft)
            {
                //Midpoint of target is outside of left or right
                return false;
            }

            return true;
        }

        private static void FindValues(FrameworkElement container, FrameworkElement target)
        {
            var containerTopLeft = container.PointToScreen(new Point());
            var itemTopLeft = target.PointToScreen(new Point());

            _topBoundary = containerTopLeft.Y;
            _bottomBoundary = _topBoundary + container.ActualHeight;
            _leftBoundary = containerTopLeft.X;
            _rightBoundary = _leftBoundary + container.ActualWidth;

            //use midpoint of item (width or height divided by 2)
            _itemLeft = itemTopLeft.X + (target.ActualWidth / 2);
            _itemTop = itemTopLeft.Y + (target.ActualHeight / 2);
        }


        private void ScalePosition(FrameworkElement element, Joint joint)
        {
            //convert the value to X/Y
            //Joint scaledJoint = joint.ScaleTo(1280, 720); 

            //convert & scale (.3 = means 1/3 of joint distance)
            Joint scaledJoint = joint.ScaleTo(1280, 720, .3f, .3f);

            Canvas.SetLeft(element, scaledJoint.Position.X);
            Canvas.SetTop(element, scaledJoint.Position.Y);

        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closing = true;
            StopKinect(kinectSensorChooser1.Kinect);
        }

        private void cpeHbAdministracionLinux_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
